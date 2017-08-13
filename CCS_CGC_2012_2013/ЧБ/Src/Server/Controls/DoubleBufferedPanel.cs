using System;
using System.Windows.Forms;


namespace Turncoat.Server.Controls
{
  public partial class DoubleBufferedPanel : Panel
  {
    public DoubleBufferedPanel() {
      InitializeComponent();
      DoubleBuffered = true;
    }
  }
}
