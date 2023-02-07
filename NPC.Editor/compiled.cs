using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Rosen.EMS.Infrastructure.DynamicConditionQuery;
using Rosen.EMS.Infrastructure.DynamicConditionQuery.Dto;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPC.Editor
{
    public partial class form_compiled : Form
    {
        private ConditionStatementDto[] compiled;
        public ConditionStatementDto[] Compiled {
            get => compiled; set {
                compiled = value;
                this.form_compiled_Shown(this, null);
            }
        }

        public form_compiled()
        {
            InitializeComponent();
        }

        private void form_compiled_Shown(object sender, EventArgs e)
        {
            this.fctb.Text = JsonConvert.SerializeObject(Compiled, Formatting.Indented);
        }

        private void btn_minify_Click(object sender, EventArgs e)
        {
            this.fctb.Text = JsonConvert.SerializeObject(Compiled, Formatting.None);
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            JToken input = JToken.FromObject(new
            {
                Condition = Compiled
            });
            var result = ConditionQueryHandler.HandleCondition(input);
            MessageBox.Show(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        private void form_compiled_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void form_compiled_VisibleChanged(object sender, EventArgs e)
        {
            this.form_compiled_Shown(sender, e);
        }

        private void btn_beautify_Click(object sender, EventArgs e)
        {
            this.fctb.Text = JsonConvert.SerializeObject(Compiled, Formatting.Indented);
        }
    }
}
