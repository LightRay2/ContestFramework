using System;
using System.Windows.Forms;


namespace Othello.Server.Controls
{
  public partial class DoubleBufferedPanel : Panel
  {
    public DoubleBufferedPanel() {
      InitializeComponent();
      DoubleBuffered = true;
    }
  }
}
