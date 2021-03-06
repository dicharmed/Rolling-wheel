﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Math;

namespace wheel
{
    public partial class Gipocycloid : Form
    {
        const float Pi = 3.14F;
        float R1, R;
        float m;
        //коодинаты точки А
        float Xa, Ya;
        //координаты центра колеса
        float Xo, Yo;
        //координаты центра неподвижного колеса
        float Xo1, Yo1;
        float Rmol1, Rmol;
        float XBase, YBase;
        float X, Y;
        float Fi, Fi1, DeltaFi, Fimax;

        private void Form4_Resize(object sender, EventArgs e)
        {
            Axis1.Left = 0;
            Axis1.Top = GroupBox1.Height;
            Axis2.Left = 0;
            Axis2.Top = Axis1.Top;
            Axis1.Width = this.DisplayRectangle.Width / 2;
            Axis2.Width = this.DisplayRectangle.Width / 2;
            Axis1.Height = Axis1.Width;
            Axis2.Height = Axis1.Width;
            Axis2.Left = Axis1.Width;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //траектория точки A
            Xa = (float)((R1 - R) * Cos(m * Fi) + R * Cos(Fi - m * Fi));
            Ya = (float)((R1 - R) * Sin(m * Fi) - R * Sin(Fi - m * Fi));
            Axis1.Pix_type = 1;
            Axis1.Pix_Size = 0; 
            Axis1.PixDraw(Xa, Ya, Color.Blue, 1);

            //колесо подвижное
            //новые координаты центра колеса
            Xo = (float)((R1 - R) * Cos(m * Fi));
            Yo = (float)((R1 - R) * Sin(m * Fi));
            Axis1.Pix_type = 3;
            Axis1.Pix_Size = 2 * R / XBase;
            Axis1.PixDraw(Xo, Yo, Color.Blue, 2); //рисуем колесо
            Axis1.Pix_color = Color.Blue;
            Axis1.Line(Xa, Ya, Xo, Yo, 2); //рисуем радиус
            Axis1.DinToPic();
            Fi = Fi + DeltaFi;
            Fi1 = Fi * m;

        }
        
        private void CommandStart_Click(object sender, EventArgs e)
        {
            Fi = 0;
            Timer1.Enabled = true;
        }

        private void CommandStop_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
        }

        private void CommandInit_Click(object sender, EventArgs e)
        {
            CommandStart.Enabled = true;

            //радиусы колес
            R1 = float.Parse(TextR1.Text,System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo); //неподвижное
            R = float.Parse(TexTR.Text, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo); //колесо подвижное

            if (R > R1)
            {
                MessageBox.Show("Радиус внутреннего колеса должен быть меньше радиуса наружного!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
    Fimax = float.Parse(TextFiMax.Text, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                DeltaFi = float.Parse(TextDeltaFi.Text, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                //размеры полей отображения
                XBase = (float)Round(R1 + 1.2);
                YBase = (float)Round(R1 + 1.2);
                InitAxis();

                //Колесо неподвижное
                Xo1 = 0;
                Yo1 = 0;
                Axis1.Pix_type = 3;
                Axis1.Pix_Size = 2 * R1 / XBase;
                Axis1.PixDraw(Xo1, Yo1, Color.Black, 1);

                //Колесо подвижное
                Xo = R1 - R;
                Yo = 0;
                Axis1.Pix_Size = 2 * R / XBase;
                Axis1.PixDraw(Xo, Yo, Color.Blue, 2);

                //Начальные координаты точки А
                Xa = R1;
                Ya = 0;
                //радиус
                Axis1.Pix_color = Color.Black;
                Axis1.Line(Xa, Ya, Xo, Yo, 2);
                Axis1.DinToPic();

                m = R / R1;
                Fi = 0;

                //Отображение траектории точки А
                while (Fi < Fimax)
                {

                    X = (float)((R1 - R) * Cos(m * Fi) + R * Cos(Fi - m * Fi));
                    Y = (float)((R1 - R) * Sin(m * Fi) - R * Sin(Fi - m * Fi));
                    Axis2.PixDraw(X, Y, Color.Black, 0);
                    Fi = Fi + DeltaFi;

                } ;

                Fi = 0;
            }


            
        }


        public Gipocycloid()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            DeltaFi = Pi / 500;
            Axis1.Width = Axis1.Height;
            Axis2.Width = Axis1.Height;
            XBase = 5;
            YBase = (float)Round(XBase / Axis1.Width * Axis1.Height);
        }

                                    //Определяет вид систем координат д/отображения 
        private void InitAxis()    //катящегося колеса(Аксис1) и траекторий точек радиуса колеса(Аксис2)
        {
            //Колесо
            Axis1.Axis_Type = 1;
            Axis1.Pix_type = 3;
            Axis1.Pix_Size = 2 / XBase;
            Axis1.x_Base = XBase;
            Axis1.y_Base = YBase;
            Axis1.AxisDraw(); //рисуем координатную ось 1

            //Траектории
            Axis2.Axis_Type = 1;
            Axis2.x_Base = XBase;
            Axis2.y_Base = YBase;
            Axis2.Pix_Size = 0;
            Axis2.AxisDraw(); //рисуем координатную ось 2
        }
    }
}
