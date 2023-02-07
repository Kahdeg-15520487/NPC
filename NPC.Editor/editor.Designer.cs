
namespace NPC.Editor
{
    partial class form_editor
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(form_editor));
            this.fctb = new FastColoredTextBoxNS.FastColoredTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tool_file = new System.Windows.Forms.ToolStripMenuItem();
            this.tool_file_new = new System.Windows.Forms.ToolStripMenuItem();
            this.tool_file_open = new System.Windows.Forms.ToolStripMenuItem();
            this.tool_file_save = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tool_edit = new System.Windows.Forms.ToolStripMenuItem();
            this.tool_edit_beautify = new System.Windows.Forms.ToolStripMenuItem();
            this.tool_edit_newline = new System.Windows.Forms.ToolStripMenuItem();
            this.tool_view = new System.Windows.Forms.ToolStripMenuItem();
            this.tool_view_runresult = new System.Windows.Forms.ToolStripMenuItem();
            this.tool_view_compiled = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_open = new System.Windows.Forms.Button();
            this.btn_compile = new System.Windows.Forms.Button();
            this.btn_run = new System.Windows.Forms.Button();
            this.btn_beautify = new System.Windows.Forms.Button();
            this.lbl_status = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fctb)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fctb
            // 
            this.fctb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fctb.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fctb.AutoIndent = false;
            this.fctb.AutoIndentChars = false;
            this.fctb.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;]+);\r\n";
            this.fctb.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.fctb.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fctb.BackBrush = null;
            this.fctb.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.fctb.CharHeight = 14;
            this.fctb.CharWidth = 8;
            this.fctb.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctb.DefaultMarkerSize = 8;
            this.fctb.DescriptionFile = "";
            this.fctb.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctb.IsReplaceMode = false;
            this.fctb.LeftBracket = '[';
            this.fctb.LeftBracket2 = '{';
            this.fctb.Location = new System.Drawing.Point(0, 56);
            this.fctb.Name = "fctb";
            this.fctb.Paddings = new System.Windows.Forms.Padding(0);
            this.fctb.RightBracket = ']';
            this.fctb.RightBracket2 = '}';
            this.fctb.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctb.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctb.ServiceColors")));
            this.fctb.Size = new System.Drawing.Size(933, 462);
            this.fctb.TabIndex = 0;
            this.fctb.WordWrapAutoIndent = false;
            this.fctb.Zoom = 100;
            this.fctb.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBox1_TextChanged);
            this.fctb.SelectionChangedDelayed += new System.EventHandler(this.fastColoredTextBox1_SelectionChangedDelayed);
            this.fctb.ScrollbarsUpdated += new System.EventHandler(this.fctb_ScrollbarsUpdated);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tool_file,
            this.tool_edit,
            this.tool_view});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(933, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "f";
            // 
            // tool_file
            // 
            this.tool_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tool_file_new,
            this.tool_file_open,
            this.tool_file_save,
            this.saveAsToolStripMenuItem});
            this.tool_file.Name = "tool_file";
            this.tool_file.Size = new System.Drawing.Size(37, 20);
            this.tool_file.Text = "File";
            // 
            // tool_file_new
            // 
            this.tool_file_new.Name = "tool_file_new";
            this.tool_file_new.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.tool_file_new.Size = new System.Drawing.Size(184, 22);
            this.tool_file_new.Text = "New";
            this.tool_file_new.Click += new System.EventHandler(this.tool_file_new_Click);
            // 
            // tool_file_open
            // 
            this.tool_file_open.Name = "tool_file_open";
            this.tool_file_open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tool_file_open.Size = new System.Drawing.Size(184, 22);
            this.tool_file_open.Text = "Open";
            // 
            // tool_file_save
            // 
            this.tool_file_save.Name = "tool_file_save";
            this.tool_file_save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tool_file_save.Size = new System.Drawing.Size(184, 22);
            this.tool_file_save.Text = "Save";
            this.tool_file_save.Click += new System.EventHandler(this.tool_file_save_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.saveAsToolStripMenuItem.Text = "Save as";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // tool_edit
            // 
            this.tool_edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tool_edit_beautify,
            this.tool_edit_newline});
            this.tool_edit.Name = "tool_edit";
            this.tool_edit.Size = new System.Drawing.Size(39, 20);
            this.tool_edit.Text = "Edit";
            // 
            // tool_edit_beautify
            // 
            this.tool_edit_beautify.Name = "tool_edit_beautify";
            this.tool_edit_beautify.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.tool_edit_beautify.Size = new System.Drawing.Size(199, 22);
            this.tool_edit_beautify.Text = "Beautify";
            // 
            // tool_edit_newline
            // 
            this.tool_edit_newline.CheckOnClick = true;
            this.tool_edit_newline.Name = "tool_edit_newline";
            this.tool_edit_newline.Size = new System.Drawing.Size(199, 22);
            this.tool_edit_newline.Text = "Newline for each clause";
            // 
            // tool_view
            // 
            this.tool_view.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tool_view_runresult,
            this.tool_view_compiled});
            this.tool_view.Name = "tool_view";
            this.tool_view.Size = new System.Drawing.Size(44, 20);
            this.tool_view.Text = "View";
            // 
            // tool_view_runresult
            // 
            this.tool_view_runresult.Name = "tool_view_runresult";
            this.tool_view_runresult.Size = new System.Drawing.Size(151, 22);
            this.tool_view_runresult.Text = "Run result";
            // 
            // tool_view_compiled
            // 
            this.tool_view_compiled.Name = "tool_view_compiled";
            this.tool_view_compiled.Size = new System.Drawing.Size(151, 22);
            this.tool_view_compiled.Text = "Compile result";
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(0, 27);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(75, 23);
            this.btn_open.TabIndex = 2;
            this.btn_open.Text = "Open";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // btn_compile
            // 
            this.btn_compile.Location = new System.Drawing.Point(158, 27);
            this.btn_compile.Name = "btn_compile";
            this.btn_compile.Size = new System.Drawing.Size(96, 23);
            this.btn_compile.TabIndex = 3;
            this.btn_compile.Text = "Compile F6";
            this.btn_compile.UseVisualStyleBackColor = true;
            this.btn_compile.Click += new System.EventHandler(this.btn_compile_Click);
            // 
            // btn_run
            // 
            this.btn_run.Location = new System.Drawing.Point(260, 27);
            this.btn_run.Name = "btn_run";
            this.btn_run.Size = new System.Drawing.Size(75, 23);
            this.btn_run.TabIndex = 4;
            this.btn_run.Text = "Run F5";
            this.btn_run.UseVisualStyleBackColor = true;
            this.btn_run.Click += new System.EventHandler(this.btn_run_Click);
            // 
            // btn_beautify
            // 
            this.btn_beautify.Location = new System.Drawing.Point(81, 27);
            this.btn_beautify.Name = "btn_beautify";
            this.btn_beautify.Size = new System.Drawing.Size(71, 23);
            this.btn_beautify.TabIndex = 5;
            this.btn_beautify.Text = "Beautify";
            this.btn_beautify.UseVisualStyleBackColor = true;
            this.btn_beautify.Click += new System.EventHandler(this.btn_beautify_Click);
            // 
            // lbl_status
            // 
            this.lbl_status.AutoSize = true;
            this.lbl_status.Location = new System.Drawing.Point(0, 525);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(0, 15);
            this.lbl_status.TabIndex = 6;
            // 
            // form_editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 549);
            this.Controls.Add(this.lbl_status);
            this.Controls.Add(this.btn_beautify);
            this.Controls.Add(this.btn_run);
            this.Controls.Add(this.btn_compile);
            this.Controls.Add(this.btn_open);
            this.Controls.Add(this.fctb);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "form_editor";
            this.Text = "NPC.Editor";
            this.Load += new System.EventHandler(this.form_editor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fctb)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox fctb;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.Button btn_compile;
        private System.Windows.Forms.Button btn_run;
        private System.Windows.Forms.ToolStripMenuItem tool_file;
        private System.Windows.Forms.ToolStripMenuItem tool_file_new;
        private System.Windows.Forms.ToolStripMenuItem tool_file_open;
        private System.Windows.Forms.ToolStripMenuItem tool_file_save;
        private System.Windows.Forms.ToolStripMenuItem tool_edit;
        private System.Windows.Forms.ToolStripMenuItem tool_edit_beautify;
        private System.Windows.Forms.ToolStripMenuItem tool_edit_newline;
        private System.Windows.Forms.ToolStripMenuItem tool_view;
        private System.Windows.Forms.ToolStripMenuItem tool_view_runresult;
        private System.Windows.Forms.ToolStripMenuItem tool_view_compiled;
        private System.Windows.Forms.Button btn_beautify;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.Label lbl_status;
    }
}

