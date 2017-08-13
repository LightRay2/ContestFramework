#!/usr/bin/env python3

import json
from math import atan2, cos, sin, sqrt, pi as PI
from random import random
from collections import namedtuple

InputData = namedtuple('InputData',
    'tick myScore enemyScore ballPosition ballTarget myPlayers enemyPlayers memory')

OutputData = namedtuple('OutputData', 'myPlayers ballTarget memory')

class defaults:
    playerRadius = 2
    ballRadius = 1
    largeSquadSize = 10
    smallSquadSize = 10
    fieldHeight = 60
    fieldWidth = 100
    attackRange = 25
    targetingStep = 1
    tolerance = 0.02

def readInput(filename):
    with open(filename, mode='r') as f:
        sTick, sMyScore, sEnemyScore = f.readline().split()
        sBallX, sBallY, sBallTargetX, sBallTargetY = f.readline().split()

        myPlayers = []
        for i in range(5):
            x, y = f.readline().split()
            myPlayers.append((float(x), float(y)))

        enemyPlayers = []
        for i in range(5):
            x, y = f.readline().split()
            enemyPlayers.append((float(x), float(y)))

        memory = f.readline().rstrip()
        if memory == '-1':
            memory = {}
        else:
            memory = json.loads(memory)
        
        return InputData(
            tick         = int(sTick),
            myScore      = int(sMyScore),
            enemyScore   = int(sEnemyScore),
            ballPosition = (float(sBallX), float(sBallY)),
            ballTarget   = (float(sBallTargetX), float(sBallTargetY)),
            myPlayers    = myPlayers,
            enemyPlayers = enemyPlayers,
            memory       = memory)

def writeOutput(filename, outputData):
    with open(filename, mode='w') as f:
        for (x, y) in outputData.myPlayers:
            f.write('%f %f\r\n' % (x, y))

        if outputData.ballTarget:
            x, y = outputData.ballTarget
            f.write('%f %f\r\n' % (x, y))

        if outputData.memory:
            f.write('memory %s\r\n' % json.dumps(outputData.memory))

            
def strategy(state):
    prevState = None
    if state.memory and 'prevState' in state.memory:
        prevState = InputData(*state.memory['prevState'])

    class newstate:
        myPlayers = state.myPlayers[:]
        squadCenter = None
        squadDirection = None
        squadSize = None
        ballTarget = None

    def frange(start, stop, step):
        val = start
        yield val
        while val < stop:
            val += step
            yield val

    def tolEq(a, b):
        return abs(a - b) < defaults.tolerance
    
    def meanPlayersPosition(players):
        x_acc, y_acc = 0, 0
        n = len(players)
        for (x, y) in players:
            x_acc += x
            y_acc += y

        return (x_acc/n, y_acc/n)

    def getDirection(posFrom, posTo):
        x1, y1 = posFrom
        x2, y2 = posTo
        return atan2(y2-y1, x2-x1)

    def getDistance(posFrom, posTo):
        x1, y1 = posFrom
        x2, y2 = posTo
        return sqrt((x2-x1)**2 + (y2-y1)**2) 

    def shift(pos, direction, distance):
        x, y = pos
        a = direction
        d = distance
        return (x + d*cos(a), y + d*sin(a))

    def weOwnTheBall():
        ballX, ballY = state.ballPosition
        for (x, y) in state.myPlayers:
            if tolEq(ballX, x) and tolEq(ballY, y):
                return True

        return False
    
    def sectorClear(position, direction, distance):
        for enemy in state.enemyPlayers:
            enemyDistance  = getDistance(position, enemy)
            enemyDirection = getDirection(position, enemy)
            shadow = atan2(defaults.playerRadius + defaults.ballRadius, distance)
            if enemyDirection - shadow <= direction <= enemyDirection + shadow:
                return False

        return True
    
    def findSafeDirection(position, lAngle, rAngle, distance):
        for direction in frange(lAngle, rAngle, 0.05):
            if not sectorClear(state.ballPosition, direction, distance):
                continue

            return direction

        return None
    
    def canSafelyPass(ballPos, playerPos):
        direction = getDirection(ballPos, playerPos)
        distance   = getDistance(ballPos, playerPos)
        return sectorClear(ballPos, direction, distance)

    def tryScore():
        ballX, ballY = state.ballPosition
        if ballX < defaults.fieldWidth - defaults.attackRange:
            return False
        
        shadows = []
        for enemy in state.enemyPlayers:
            (enemyX, enemyY) = enemy
            distance = getDistance(state.ballPosition, enemy)
            direction = atan2(enemyY-ballY, enemyX-ballX)
            shadowSize = atan2(defaults.playerRadius + defaults.ballRadius, distance)
            shadows.append((direction - shadowSize, direction + shadowSize))

        targetX = defaults.fieldWidth
        for targetY in frange(0, defaults.fieldHeight, defaults.targetingStep):
            distance = getDistance(state.ballPosition, (targetX, targetY))
            if distance > defaults.attackRange:
                continue

            direction = atan2(targetY-ballY, targetX-ballX)
            if any(map(lambda r: r[0] <= direction <= r[1], shadows)):
                continue

            newstate.ballTarget = (targetX, targetY)
            return True

        return False

    def tryStepForward():
        direction = findSafeDirection(state.ballPosition, -PI+0.1, PI-0.1, distance=5)
        if not direction:
            return False
        
        newstate.squadDirection = direction
        newstate.squadCenter = shift(newstate.squadCenter, direction, distance=2.5)
        return True

    
    def trySaveTheBall():
        for myPlayer in state.myPlayers:
            if not canSafelyPass(state.ballPosition, myPlayer):
                continue

            newstate.ballTarget = myPlayer
            return True

        return False


    def tryStepBackward():
        direction = findSafeDirection(state.ballPosition, PI-0.5, PI+0.5, distance=5)
        if not direction:
            return False
        
        newstate.squadDirection = direction
        newstate.squadCenter = shift(squadCenter, direction, distance=2.5)
        return True
        

    def tryThrowTheBall():
        direction = findSaveDirection(state.ballPosition, -PI-0.2, PI+0.2, distance=20)
        if not direction:
            return False

        newstate.ballTarget = shift(state.ballPosition, direction, 20)
        return True
        

    def panic():
        direction = -PI + 2*PI*random()
        distance = random() * 30
        newstate.ballTarget = shift(state.ballPosition, direction, distance)
        return True

    def moveTowardTheBall():
        newstate.squadDirection = getDirection(newstate.squadCenter, state.ballTarget)
        newstate.squadCenter = state.ballTarget
    
    def attack():
        squadSize = defaults.largeSquadSize
        tryScore() or \
            tryStepForward() or \
            trySaveTheBall() or \
            panic()

        #   tryThrowTheBall() or \
        #tryStepBackward() or \


    def getTheBall():
        moveTowardTheBall()
        adjustSquad()
        newstate.myPlayers[1] = state.ballTarget
        newstate.myPlayers[0] = state.ballTarget

    def adjustSquad():
        a = newstate.squadDirection
        l = newstate.squadSize
        cosa, sina = l*cos(a), l*sin(a)
        x0, y0 = newstate.squadCenter
        
        newstate.myPlayers = [
            (-sina+x0,  cosa+y0),
            (-cosa+x0, -sina+y0),
            ( sina+x0, -cosa+y0),
            ( x0,       y0),
            ( cosa+x0,  sina+y0)]
        
    # Remember: we're a squad.
    if state.memory:
        newstate.squadDirection = state.memory['squadDirection']
        newstate.squadSize      = state.memory['squadSize']
    else:
        newstate.squadDirection = 0
        newstate.squadSize = defaults.largeSquadSize

    newstate.squadCenter = meanPlayersPosition(state.myPlayers)

    if weOwnTheBall():
        attack()
        adjustSquad()
    else:
        getTheBall()
        
    # ===
    if 'prevState' in state.memory:
        del state.memory['prevState']
        
    return OutputData(
        myPlayers    = newstate.myPlayers,
        ballTarget   = newstate.ballTarget,
        memory       = dict(
            prevState = state,
            squadDirection = newstate.squadDirection,
            squadSize = newstate.squadSize))

if __name__ == '__main__':
    inputData = readInput('input.txt')
    outputData = strategy(inputData)
    writeOutput('output.txt', outputData)
