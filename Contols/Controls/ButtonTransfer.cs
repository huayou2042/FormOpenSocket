using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controls.Entity;

namespace Controls.Controls
{
    public partial class ButtonTransfer : BaseControl
    {
        public ButtonTransfer()
        {
            InitializeComponent();
            fontText = base.Font;
        }

        [Browsable(true)]
        [Category("Customize")]
        [Description("数据长度")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ushort Length { get; set; }

        [Browsable(true)]
        [Category("Customize")]
        [Description("十六进制数据")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Hex { get; set; } = string.Empty;

        [Browsable(true)]
        [Category("Customize")]
        [Description("功能码")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FunctionId { get; set; } = string.Empty;

        string _text = string.Empty;
        [Browsable(true)]
        [Category("Customize")]
        [Description("控件的显示文本")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text { get { return _text; } set { _text = value; this.Refresh(); } }

        Font fontText;
        [Browsable(true)]
        [Category("Customize")]
        [Description("控件的显示字体")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Font TextFont { get { return fontText; } set { fontText = value; this.Refresh(); } }

        bool isOn = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            isOn = true;
            Refresh();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isOn = false;
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //// 画背景 渐变 
            if (isOn)
            {
                Graphics graphics = e.Graphics;
                LinearGradientMode _linearGradientMode = LinearGradientMode.Vertical;
                Color _brushColorStart = ColorTranslator.FromHtml("#AAAAAA");
                Color _brushColorEnd = ColorTranslator.FromHtml("#777777");
                Brush bush = new LinearGradientBrush(ClientRectangle, _brushColorStart, _brushColorEnd, _linearGradientMode);
                graphics.FillRectangle(bush, ClientRectangle);

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

                e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(255, 255, 255)), new Rectangle(0, 0, this.Width - 1, this.Height - 1));
                System.Drawing.SizeF sizeEnd = e.Graphics.MeasureString(Text, fontText);

                e.Graphics.DrawString(Text, fontText, new SolidBrush(ForeColor), new PointF((this.Width - sizeEnd.Width) / 2, (this.Height - sizeEnd.Height) / 2 + 1));
            }
            else
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

                //e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(32, 30, 99)), new Rectangle(0, 0, this.Width - 1, this.Height - 1));
                System.Drawing.SizeF sizeEnd = e.Graphics.MeasureString(Text, fontText);

                e.Graphics.DrawString(Text, fontText, new SolidBrush(ForeColor), new PointF((this.Width - sizeEnd.Width) / 2, (this.Height - sizeEnd.Height) / 2 + 1));
            }
        }
    }
}
