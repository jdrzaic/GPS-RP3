using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPS.Views
{
    public class LocationNodeButton : RadioButton
    {
        public GPSGraph.Node node { get; set; }
        public GraphMainForm creator { get; set; }

        public LocationNodeButton(Color outerColor, Color innerColor) : this(outerColor, innerColor, 24, 24) {}

        public LocationNodeButton(Color outerColor, Color innerColor, int width, int height) : base()
        {
            this.Size = new Size(width, height);
            this.Text = "";
            this.OuterColor = outerColor;
            this.InnerColor = innerColor;
            this.AutoSize = false;
            Font = new Font("Arial", 8);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            this.MouseClick += LocationNodeButton_MouseClick;
            ContextMenu menu = new ContextMenu();
            MenuItem itemShow = menu.MenuItems.Add("Show node details");
            itemShow.Click += ItemShow_Click;
            MenuItem itemAddCharacteristic = menu.MenuItems.Add("Add characteristic");
            itemAddCharacteristic.Click += ItemAddCharacteristic_Click;
            MenuItem itemConnectOne = menu.MenuItems.Add("Connect one way");
            itemConnectOne.Click += ItemConnectOne_Click;
            MenuItem itemConnectBoth = menu.MenuItems.Add("Connect both ways");
            itemConnectBoth.Click += ItemConnectBoth_Click;
            MenuItem itemFindShortestWithCriteria = menu.MenuItems.Add("Find shortest path with criteria");
            itemFindShortestWithCriteria.Click += ItemFindShortestWithCriteria_Click;
            MenuItem itemFindShortest = menu.MenuItems.Add("Find shortest path");
            itemFindShortest.Click += ItemFindShortest_Click;
            this.ContextMenu = menu;
        }

        private void ItemFindShortest_Click(object sender, EventArgs e)
        {
            if (!CheckNodeSelected()) return;
            this.creator.CalculateShortestPath(this.node);
        }

        private void ItemFindShortestWithCriteria_Click(object sender, EventArgs e)
        {
            if (!CheckNodeSelected()) return;
            var addCriteriaDialog = new AddCriteriasDialog(this);
            addCriteriaDialog.ShowDialog();
        }

        private void ItemConnectBoth_Click(object sender, EventArgs e)
        {
            if (!CheckNodeSelected()) return;
            this.creator.CreateBothWayConnection(this.node);
        }

        public void ItemFindShortestWithCriteriaCallback(List<string> types,
            List<string> names)
        {
            this.creator.CalculateShortestPathWithCriteria(this.node,
                new Tuple<List<string>, List<string>>(types, names));
        }

        private void ItemConnectOne_Click(object sender, EventArgs e)
        {
            if (!CheckNodeSelected()) return;
            this.creator.CreateOneWayConnection(this.node);
        }

        private bool CheckNodeSelected()
        {
            if (!this.creator.nodeSelected || this.creator.lastSelectedNode == this.node)
            {
                MessageBox.Show("No origin node is selected", "No node");
                return false;
            }
            return true;
        }

        private void ItemAddCharacteristic_Click(object sender, EventArgs e)
        {
            var addCharacteristicDialog = new AddNodeCharacteristicDialog(node);
            addCharacteristicDialog.ShowDialog();
        }

        private void ItemShow_Click(object sender, EventArgs e)
        {
            var showDetailsDialog = new NodeDetailForm(node);
            showDetailsDialog.ShowDialog(); 
        }

        private void LocationNodeButton_MouseClick(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Right)
            {
            }
            else if (me.Button == MouseButtons.Left)
            {
                // checking if ctrl clicked
                if (ModifierKeys.HasFlag(Keys.Control))
                {
                    this.creator.SelectNode(this.node);
                } else
                {
                    var basicForm = new NodeBasicForm(this.node);
                    basicForm.ShowDialog();
                }
            }
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
            g.FillEllipse(new SolidBrush(this.OuterColor), innerRect);

            Rectangle outerRect = new Rectangle(0, 0, diameter, diameter);
            g.DrawEllipse(new Pen(Color.Transparent), outerRect);

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
