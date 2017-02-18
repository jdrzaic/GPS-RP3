using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPS.Views
{
    public class LocationNodeButton : RadioButton
    {
        public GPSNode node;
        public LocationNodeButton() : base()
        {
            this.Text = "";
            OuterColor = Color.Red;
            InnerColor = Color.Red;
            this.AutoSize = false;
            Font = new Font("Arial", 8.75F);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }
        public Color InnerColor { get; set; }
        public Color OuterColor { get; set; }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            PaintParentBackground(pevent);
            int diameter = ClientRectangle.Height - 1;

            RectangleF innerRect = new RectangleF(1F, 1F, diameter - 2, diameter - 2);
            g.FillEllipse(new SolidBrush(Color.Red), innerRect);

            Rectangle outerRect = new Rectangle(0, 0, diameter, diameter);
            g.DrawEllipse(new Pen(OuterColor), outerRect);

            if (Checked)
            {
                innerRect.Inflate(-3, -3);
                g.FillEllipse(new SolidBrush(InnerColor), innerRect);
            }

            g.DrawString(Text, Font, new SolidBrush(ForeColor), diameter + 5, 1);
        }

        private void PaintParentBackground(PaintEventArgs e)
        {
            if (Parent == null)
            {
                e.Graphics.FillRectangle(SystemBrushes.Control, ClientRectangle);
                return;
            }


            Rectangle rect = new Rectangle(Left, Top, Width, Height);
            e.Graphics.TranslateTransform(-rect.X, -rect.Y);

            try
            {
                using (PaintEventArgs pea = new PaintEventArgs(e.Graphics, rect))
                {
                    pea.Graphics.SetClip(rect);
                    InvokePaintBackground(Parent, pea);
                    InvokePaint(Parent, pea);
                }
            }
            finally
            {
                e.Graphics.TranslateTransform(rect.X, rect.Y);
            }
        }
    }
}
