namespace GPS.Views
{
    partial class GraphMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calculateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shortestPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shortestWithCriteriaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cleanGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.calculateToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(584, 24);
            this.menuStrip2.TabIndex = 1;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNodeToolStripMenuItem,
            this.cleanGraphToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // addNodeToolStripMenuItem
            // 
            this.addNodeToolStripMenuItem.Name = "addNodeToolStripMenuItem";
            this.addNodeToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.addNodeToolStripMenuItem.Text = "Add Node";
            // 
            // calculateToolStripMenuItem
            // 
            this.calculateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shortestPathToolStripMenuItem,
            this.shortestWithCriteriaToolStripMenuItem});
            this.calculateToolStripMenuItem.Name = "calculateToolStripMenuItem";
            this.calculateToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.calculateToolStripMenuItem.Text = "Calculate";
            // 
            // shortestPathToolStripMenuItem
            // 
            this.shortestPathToolStripMenuItem.Name = "shortestPathToolStripMenuItem";
            this.shortestPathToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.shortestPathToolStripMenuItem.Text = "Shortest Path";
            // 
            // shortestWithCriteriaToolStripMenuItem
            // 
            this.shortestWithCriteriaToolStripMenuItem.Name = "shortestWithCriteriaToolStripMenuItem";
            this.shortestWithCriteriaToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.shortestWithCriteriaToolStripMenuItem.Text = "Shortest Path with Criteria";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userManualToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // userManualToolStripMenuItem
            // 
            this.userManualToolStripMenuItem.Name = "userManualToolStripMenuItem";
            this.userManualToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.userManualToolStripMenuItem.Text = "User manual";
            // 
            // cleanGraphToolStripMenuItem
            // 
            this.cleanGraphToolStripMenuItem.Name = "cleanGraphToolStripMenuItem";
            this.cleanGraphToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.cleanGraphToolStripMenuItem.Text = "Clean Graph";
            // 
            // GraphMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(584, 561);
            this.Controls.Add(this.menuStrip2);
            this.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Name = "GraphMainForm";
            this.Text = "Edit Graph";
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calculateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shortestPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shortestWithCriteriaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userManualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cleanGraphToolStripMenuItem;
    }
}