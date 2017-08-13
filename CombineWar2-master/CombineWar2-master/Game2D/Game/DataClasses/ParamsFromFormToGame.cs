using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Game2D.Game
{
    public class ParamsFromMainFormToGame
    {
        public string replayPath=null;

        [XmlIgnore]
        public bool ThisIsReplayGame = false;

        public string javaPath = null;
        /// <summary>
        /// от 10 до 200
        /// </summary>
        public int AnimationSpeedInPercent;

        /// <summary>
        /// проверяем сначала реплейна null, если нет, для каждого игрока проверяем, человек ли, и если нет, берем адрес
        /// </summary>
        /// 
        //todo replay
        public string replayFileAddress=null;
        public bool firstIsControlledByHuman, secondIsControlledByHuman;
        public string firstProgramAddress, secondProgramAddress;

        /// <summary>
        /// не использовать
        /// </summary>
        public string lastOpenFileDialog;
        public int Brightness;

        public void AddToList(List<ParamsFromMainFormToGame> list)
        {
            list.Add(this.MemberwiseClone() as ParamsFromMainFormToGame);
        }
    }
}
