﻿using FastColoredTextBoxNS;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NPC.Editor;
using NPC.Runtime.Runtime;

using Rosen.EMS.Infrastructure.DynamicConditionQuery;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPC.Editor
{
    public partial class form_editor : Form
    {
        //styles
        TextStyle KeywordStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
        TextStyle AltStringStyle = new TextStyle(Brushes.Orange, null, FontStyle.Bold);
        TextStyle LiteralValue = new TextStyle(Brushes.ForestGreen, null, FontStyle.Regular);
        TextStyle CommentStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        TextStyle StringStyle = new TextStyle(Brushes.Brown, null, FontStyle.Italic);
        TextStyle ErrorStyle = new TextStyle(null, null, FontStyle.Underline);
        MarkerStyle SameWordsStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(40, Color.Gray)));

        //stuff
        private bool isCtrlKIssued;

        //dialog
        private OpenFileDialog openFileDialog;
        private readonly PrettyPrint prettyPrinter;
        private readonly Translator translator;

        //child form
        private form_compiled formCompiled;

        public form_editor()
        {
            InitializeComponent();

            openFileDialog = new OpenFileDialog();

            tool_file_new.Click += (o, e) => { };
            tool_file_open.Click += this.btn_open_Click;
            tool_file_save.Click += (o, e) => { };
            tool_edit_beautify.Click += this.btn_beautify_Click;
            tool_view_compiled.Click += (o, e) => { };
            tool_view_runresult.Click += (o, e) => { };

            prettyPrinter = new PrettyPrint();
            translator = new Translator();
            formCompiled = new form_compiled();
            formCompiled.Hide();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.K))
            {
                this.isCtrlKIssued = true;
                return true;
            }
            if (this.isCtrlKIssued && keyData == (Keys.Control | Keys.D))
            {
                this.isCtrlKIssued = false;
                this.btn_beautify_Click(this, null);
                return true;
            }

            if (keyData == Keys.F5)
            {
                this.btn_run_Click(this, null);
                return true;
            }

            if (keyData == Keys.F6)
            {
                this.btn_compile_Click(this, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

//        protected override void OnLoad(EventArgs e)
//        {
//            //fctb.Text = File.ReadAllText("sample.npc");
//            fctb.Text = @"if (int 1 == 2){
//    ""a"" = ""b""
//} else {
//    ""a"" = ""c""
//}";
//            base.OnLoad(e);
//        }

        private void fastColoredTextBox1_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            fctb.LeftBracket = '(';
            fctb.RightBracket = ')';
            fctb.LeftBracket2 = '{';
            fctb.RightBracket2 = '}';
            //clear style of changed range
            e.ChangedRange.ClearStyle(KeywordStyle, LiteralValue, CommentStyle, StringStyle, AltStringStyle, ErrorStyle);

            //string highlighting
            e.ChangedRange.SetStyle(StringStyle, @"""""|@""""|''|@"".*?""|(?<!@)(?<range>"".*?[^\\]"")|'.*?[^\\]'");
            //guid and datetime highlighting
            e.ChangedRange.SetStyle(AltStringStyle, @"(?<!@)(?<range>(g|d))("".*?[^\\]"")");
            //comment highlighting
            e.ChangedRange.SetStyle(CommentStyle, @"//.*$", RegexOptions.Multiline);
            //number and boolean highlighting
            e.ChangedRange.SetStyle(LiteralValue, @"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b|true|false");
            //keyword highlighting
            e.ChangedRange.SetStyle(KeywordStyle, @"\b(if|elif|else|and|or|not|int|str|bool|guid|datetime)\b");

            //clear folding markers
            e.ChangedRange.ClearFoldingMarkers();

            //set folding markers
            e.ChangedRange.SetFoldingMarkers("{", "}");//allow to collapse brackets block
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var fileContent = File.ReadAllText(openFileDialog.FileName);
                    fctb.Text = fileContent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening selected file", ex.Message);
                }
            }
        }

        private void btn_compile_Click(object sender, EventArgs e)
        {
            (NPC.Compiler.AST.Policy policy, NPC.Compiler.Error error) = NPC.Compiler.Compiler.Compile(fctb.Text);
            if (policy != null)
            {
                var translated = translator.Translate(policy);

                formCompiled.Compiled = translated;
                formCompiled.Show();
            }
            else
            {
                //MessageBox.Show(error.Message);
                var errorLine = new FastColoredTextBoxNS.Range(fctb, error.Line);
                errorLine.SetStyle(ErrorStyle);
            }
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            (NPC.Compiler.AST.Policy policy, NPC.Compiler.Error error) = NPC.Compiler.Compiler.Compile(fctb.Text);
            var translated = translator.Translate(policy);

            JToken input = JToken.FromObject(new
            {
                Condition = translated
            });
            var result = ConditionQueryHandler.HandleCondition(input);
            MessageBox.Show(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        private void btn_beautify_Click(object sender, EventArgs e)
        {
            bool isNewline = tool_edit_newline.Checked;

            (NPC.Compiler.AST.Policy policy, NPC.Compiler.Error error) = NPC.Compiler.Compiler.Compile(fctb.Text);

            fctb.Text = prettyPrinter.Beautify(policy, isNewline);
        }

        private void fastColoredTextBox1_SelectionChangedDelayed(object sender, EventArgs e)
        {
            fctb.VisibleRange.ClearStyle(SameWordsStyle);
            if (!fctb.Selection.IsEmpty)
            {
                return;//user selected diapason
            }

            //get fragment around caret
            var fragment = fctb.Selection.GetFragment(@"\w");
            string text = fragment.Text;
            if (text.Length == 0)
            {
                return;
            }
            //highlight same words
            var ranges = fctb.VisibleRange.GetRanges(@"\b" + text + @"\b").ToArray();
            if (ranges.Length > 1)
            {
                foreach (var r in ranges)
                {
                    r.SetStyle(SameWordsStyle);
                }
            }
        }

        private void fctb_ScrollbarsUpdated(object sender, EventArgs e)
        {
            fastColoredTextBox1_SelectionChangedDelayed(sender, e);
        }
    }
}
