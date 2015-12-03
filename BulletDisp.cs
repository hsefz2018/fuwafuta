using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TwoDoubleThree {
    // http://stackoverflow.com/questions/19842722/setting-a-font-with-outline-color-in-c-sharp
    public class CustomLabel : Label {
        public CustomLabel() {
            OutlineForeColor = Color.Green;
            OutlineWidth = 2;
        }
        public Color OutlineForeColor { get; set; }
        public float OutlineWidth { get; set; }
        protected override void OnPaint(PaintEventArgs e) {
            e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            GraphicsPath gp = new GraphicsPath();
            Pen outline = new Pen(OutlineForeColor, OutlineWidth) { LineJoin = LineJoin.Round };
            StringFormat sf = new StringFormat();
            Brush foreBrush = new SolidBrush(ForeColor);
            Rectangle r = this.ClientRectangle;
            gp.AddString(this.Text, Font.FontFamily, (int)Font.Style, Font.Size, r, sf);
            e.Graphics.ScaleTransform(1.3f, 1.35f);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.DrawPath(outline, gp);
            e.Graphics.FillPath(foreBrush, gp);
        }
    }
    
    public class BulletDisp : Control {
        public static String FontName = "华文黑体";
        protected CustomLabel label;
        public static Color BackgroundColor = Color.FromArgb(128, 128, 128);
        public static Color BackgroundReplacement = Color.FromArgb(126, 126, 126);
        
        public BulletDisp() {
            this.InitializeComponent();
            this.DoubleBuffered = true;
        }
        
        private void InitializeComponent() {
            label = new CustomLabel();
            label.Location = new Point(0, 0);
            label.AutoSize = true;
            label.OutlineForeColor = Color.Black;
            this.Controls.Add(label);

            this.BackColor = BackgroundColor;
            this.TextColor = Color.White;
            this.FontSize = 36;
            this.OutlineWidth = 2;
            this.Text = "";
        }
        
        public Color TextColor {
            get { return label.ForeColor; }
            set {
                if (Math.Abs(value.R - BackgroundColor.R) < 2
                    && Math.Abs(value.G - BackgroundColor.G) < 2
                    && Math.Abs(value.B - BackgroundColor.B) < 2)
                {
                    value = BackgroundReplacement;
                }
                label.ForeColor = value;
            }
        }
        
        public new String Text {
            get { return label.Text; }
            set {
                label.Text = value.Replace(' ', '　');
                this.Size = label.Size;
            }
        }
        
        public Single FontSize {
            get { return label.Font.Size; }
            set {
                label.Font = new Font(BulletDisp.FontName, value);
                this.Size = label.Size;
            }
        }
        
        public float OutlineWidth {
            get { return label.OutlineWidth; }
            set { label.OutlineWidth = value; }
        }

        public static BulletDisp Fire(String text, Color color, int x, int y) {
            BulletDisp ret = new BulletDisp();
            ret.Text = text;
            ret.TextColor = color;
            ret.Location = new Point(x, y);
            ret.Hide();
            return ret;
        }
    }
    
    public static class Test {
        public static void Main() {
            DanmakuPool pool = new DanmakuPool();
            pool.Fire(BulletType.TOP_SLIDING, 0, "こんにちは世界 おおおおおおおおはよう", Color.White);
            pool.Fire(BulletType.TOP_SLIDING, 0, "Hello World 1!", Color.Black);
            pool.Fire(BulletType.TOP_SLIDING, 1, "(~~=u=)~~", Color.Lime);
            pool.Fire(BulletType.TOP_SLIDING, 3, "Hello World 2!", Color.Gray);
            pool.Fire(BulletType.TOP_SLIDING, 2, "Hello World 3!", Color.Yellow);
            pool.Fire(BulletType.TOP_SLIDING, 7, "Hello World 4!", Color.Red);
            pool.Fire(BulletType.TOP_SLIDING, 5, "にゃんぱすー", Color.Magenta);
            Random r = new Random();
            for (int i = 0; i < 30; ++i) {
                int nowTicks = (int)(DateTime.Now.Ticks % int.MaxValue);
                pool.Fire(BulletType.TOP_SLIDING, i * 0.12 + 0.12, "+++++++++++++++++xxxx+++++++++++++", Color.PowderBlue);
                pool.Fire(BulletType.TOP_SLIDING, 6 + i * 0.1, "++++++++++++++++++++++++++++++", Color.FromArgb(r.Next() % 256, r.Next() % 256, r.Next() % 256));
                pool.Fire(BulletType.BOTTOM_STICKY, 2.6 + i * 0.1, "++++++++++++++++++++++++++++++", Color.Magenta);
                pool.Fire(BulletType.TOP_STICKY, i * 0.26, "aaaa " + DateTime.Now.AddSeconds(i * 0.3).Ticks, Color.FromArgb(r.Next() % 256, r.Next() % 256, r.Next() % 256));
                pool.Fire(BulletType.BOTTOM_STICKY, i * 0.3, "NANNNNN" + r.Next() + "Zzz", Color.FromArgb(r.Next() % 256, r.Next() % 256, r.Next() % 256));
            }
            Application.Run(pool.RepresentativeForm());
        }
    }
}
