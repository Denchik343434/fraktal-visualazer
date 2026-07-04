namespace Fractal_Visualizer
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer;
        public Form1()
        {
            InitializeComponent();
            comboBoxFractalType.SelectedIndex = 0;
            comboBoxPallete.SelectedIndex = 0;
            checkBoxAutoCalculating.Checked = true;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 50;
            timer.Tick += ResizeTimer_Tick;
        }

        //Методы обьектов интерфейса

        //*** методы событий 
        private void UI_SelectedValuesChanged(object sender, EventArgs e)
        {
            bool autoCalculating = checkBoxAutoCalculating.Checked;

            if (autoCalculating)
            {
                string slectedFractal = comboBoxFractalType.Text;
                ComboBoxFractalTypeRenderMetod(slectedFractal);
            }
        }

        private void comboBoxFractalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control control in panelControl.Controls)
            {
                if (control is Panel)
                    control.Visible = false;
            }
            string slectedFractal = comboBoxFractalType.Text;
            ComboBoxFractalTypeMetod(slectedFractal);
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            string slectedFractal = comboBoxFractalType.Text;
            ComboBoxFractalTypeRenderMetod(slectedFractal);
        }

        //*** перерисовка после изменения размера окна
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                return;

            timer.Stop();
            timer.Start();
        }

        private void ResizeTimer_Tick(object? sender, EventArgs e)
        {
            timer.Stop();
            int pictureBoxWidth = ClientSize.Width - 240;
            pictureBoxFractal.Width = pictureBoxWidth;
            string slectedFractal = comboBoxFractalType.Text;
            ComboBoxFractalTypeRenderMetod(slectedFractal);
        }

        //*** для блокировки интерфейса во время рассчёта
        public void SetControlsEnabled(bool isEnabled)
        {
            SetControlsEnabled(this, isEnabled);
        }

        private void SetControlsEnabled(Control parentControl, bool isEnabled)
        {
            foreach (Control control in parentControl.Controls)
            {
                control.Enabled = isEnabled;

                if (control.HasChildren)
                {
                    SetControlsEnabled(control, isEnabled);
                }
            }
        }

        //*** для отслеживания прогресса рассчёта 
        private void CalculatingProgress(int width, int x)
        {
            int progress = (int)(x * 100 / width);

            if (labelCalculatingProgress.InvokeRequired)
            {
                labelCalculatingProgress.Invoke(new Action(() =>
                {
                    labelCalculatingProgress.Text = $"рассчёт: {progress}%";
                }));
            }
        }

        // Методы подготавливающие интерфейс и запускающие рассчёт при выборе в комбобоксе выбора фракталов
        private void MandelbrotSetMethod()
        {
            RenderMandelbrotSet();
        }

        private void JuliaSetMethod()
        {
            panelJuliaSet.Visible = true;
            RenderJuliaSet();
        }

        private void GeneralizedMandelbrotSetMetod()
        {
            panelGeneralizedMandelbrot.Visible = true;
            RenderGeneralizedMandelbrotSet();
        }

        private void GeneralizedJuliaSetMethod()
        {
            panelGeneralizedJulia.Visible = true;
            RenderGeneralizedJuliaSet();
        }

        private void BlotMethod()
        {
            panelBlot.Visible = true;
            RenderBlot();
        }

        private void BirningShipMethod()
        {
            panelBirningShip.Visible = true;
            RenderBirningShip();
        }

        private void ReversiveBirningShipMethod()
        {
            panelReversiveBirningShip.Visible= true;
            RenderReversiveBirningShip();
        }



        //Методы проверки значений комбобокса 
        private void ComboBoxFractalTypeMetod(string selectedFractal)
        {
            if (selectedFractal == "Множество Мандельброта")
                MandelbrotSetMethod();

            if (selectedFractal == "Множество Жюлиа")
                JuliaSetMethod();

            if (selectedFractal == "Обобщённый Мандельброт")
                GeneralizedMandelbrotSetMetod();

            if (selectedFractal == "Обобщённый Жюлиа")
                GeneralizedJuliaSetMethod();

            if (selectedFractal == "Клякса")
                BlotMethod();

            if (selectedFractal == "Горящий корабль")
                BirningShipMethod();

            if (selectedFractal == "Обратный горящий корабль")
                ReversiveBirningShipMethod();

        }

        private void ComboBoxFractalTypeRenderMetod(string selectedFractal)
        {
            if (selectedFractal == "Множество Мандельброта")
                RenderMandelbrotSet();

            if (selectedFractal == "Множество Жюлиа")
                RenderJuliaSet();

            if (selectedFractal == "Обобщённый Мандельброт")
                RenderGeneralizedMandelbrotSet();

            if (selectedFractal == "Обобщённый Жюлиа")
                RenderGeneralizedJuliaSet();

            if (selectedFractal == "Клякса")
                RenderBlot();

            if (selectedFractal == "Горящий корабль")
                RenderBirningShip();

            if (selectedFractal == "Обратный горящий корабль")
                RenderReversiveBirningShip();
        }

        private void ComboBoxPalleteMetod(string selectedPallete, int x, int y, int iterations, int iterationsAmount, Bitmap bmp, Complex z)
        {
            if (selectedPallete == "Белый фон")
                WhiteBackground(x, y, iterations, iterationsAmount, bmp);

            else if (selectedPallete == "Синий градиент")
                BlueGradient(x, y, iterations, iterationsAmount, bmp);

            else if (selectedPallete == "Синий градиент (ярче)")
                BlueGradientLight1(x, y, iterations, iterationsAmount, bmp);

            else if (selectedPallete == "Синий градиент (ещё ярче)")
                BlueGradientLight2(x, y, iterations, iterationsAmount, bmp);
            else if (selectedPallete == "Серые области скоростей")
                GraySpeedZones(x, y, bmp, z);
            else if (selectedPallete == "Радужные области")
                RainbowSpeedZones(x, y, bmp, z);
            else if (selectedPallete == "Фиолетово-чёрные области")
                PurpleBlackSpeedZones(x, y, bmp, z);
        }


        // Методы расчёта и рисования фракталов
        private async void RenderMandelbrotSet()
        {
            int width = pictureBoxFractal.Width;
            int height = pictureBoxFractal.Height;
            int iterationsAmount = (int)numericUpDownIterations.Value;
            string selectedPallete = comboBoxPallete.Text;

            double yMin = -1.5;
            double yMax = 1.5;
            double fieldSizeX = 3 * ((double)width / height);
            double xMax = 1.0 / 3.5 * fieldSizeX;
            double xMin = -fieldSizeX / 3.5 * 2.5;

            SetControlsEnabled(false);

            Bitmap result = await Task.Run(() =>
            {
                Bitmap bmp = new Bitmap(width, height);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        double cReal = xMin + ((double)x / width) * (xMax - xMin);
                        double cImag = yMin + ((double)y / height) * (yMax - yMin);

                        Complex c = new Complex(cReal, cImag);
                        Complex z = new Complex(0, 0);
                        int iterations = 0;

                        while (iterations < iterationsAmount && z.SquaredAbs < 4)
                        {
                            z = Complex.Square(z);
                            z = Complex.Sum(c, z);
                            iterations++;
                        }
                        ComboBoxPalleteMetod(selectedPallete, x, y, iterations, iterationsAmount, bmp, z);
                    }

                    CalculatingProgress(width, x);
                }

                return bmp;
            });
            labelCalculatingProgress.Text = "рассчёт: завершён";
            SetControlsEnabled(true);
            pictureBoxFractal.Image = result;
        }

        private async void RenderJuliaSet()
        {
            int width = pictureBoxFractal.Width;
            int height = pictureBoxFractal.Height;
            int iterationsAmount = (int)numericUpDownIterations.Value;
            double cReal = (double)numericUpDownJuliaCReal.Value;
            double cImag = (double)numericUpDownJuliaCImage.Value;
            string selectedPallete = comboBoxPallete.Text;

            double yMin = -1.5;
            double yMax = 1.5;
            double fieldSizeX = 3 * ((double)width / height);
            double xMax = 0.5 * fieldSizeX;
            double xMin = -fieldSizeX * 0.5;

            SetControlsEnabled(false);

            Bitmap result = await Task.Run(() =>
            {
                Bitmap bmp = new Bitmap(width, height);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        double zReal = xMin + ((double)x / width) * (xMax - xMin);
                        double zImag = yMin + ((double)y / height) * (yMax - yMin);

                        int iterations = 0;

                        Complex z = new Complex(zReal, zImag);
                        Complex c = new Complex(cReal, cImag);

                        while (iterations < iterationsAmount && z.SquaredAbs < 4)
                        {
                            z = Complex.Square(z);
                            z = Complex.Sum(z, c);
                            iterations++;
                        }
                        ComboBoxPalleteMetod(selectedPallete, x, y, iterations, iterationsAmount, bmp, z);
                    }
                    CalculatingProgress(width, x);
                }

                return bmp;
            });
            labelCalculatingProgress.Text = "рассчёт: завершён";
            SetControlsEnabled(true);
            pictureBoxFractal.Image = result;
        }

        private async void RenderGeneralizedMandelbrotSet()
        {
            int width = pictureBoxFractal.Width;
            int height = pictureBoxFractal.Height;
            int iterationsAmount = (int)numericUpDownIterations.Value;
            double generalYFieldChange = (double)numericUpDownGeneralizedMandelbrotYFieldChange.Value;
            double generalFieldSize = (double)numericUpDownGeneralizedMandelbrotFieldSize.Value;
            double generalXFieldChange = (double)numericUpDownGeneralizedMandelbrotXFieldChange.Value;
            double power = (double)numericUpDownGeneralizedMandelbrotPower.Value;
            double generalVariableReal = (double)numericUpDownGeneralizedMaidelbrotZ0Real.Value;
            double generalVariableImage = (double)numericUpDownGeneralizedMandelbrotZ0Image.Value;
            string selectedPallete = comboBoxPallete.Text;

            double yMin = -1.5 - generalYFieldChange - generalFieldSize * 0.5;
            double yMax = 1.5 - generalYFieldChange + generalFieldSize * 0.5;
            double fieldSizeX = (yMax - yMin) * ((double)width / height);
            double xMax = 1.0 / 3.5 * fieldSizeX + generalXFieldChange;
            double xMin = -fieldSizeX * 2.5 / 3.5 + generalXFieldChange;

            SetControlsEnabled(false);

            Bitmap result = await Task.Run(() =>
            {
                Bitmap bmp = new Bitmap(width, height);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        double cReal = xMin + ((double)x / width) * (xMax - xMin);
                        double cImag = yMin + ((double)y / height) * (yMax - yMin);

                        double zReal = generalVariableReal;
                        double zImag = generalVariableImage;

                        int iterations = 0;

                        Complex z = new Complex(zReal, zImag);
                        Complex c = new Complex(cReal, cImag);

                        if (power == 2)
                        {
                            while (iterations < iterationsAmount && z.SquaredAbs < 4)
                            {
                                z = Complex.Square(z);
                                z = Complex.Sum(z, c);
                                iterations++;
                            }
                        }
                        else
                        {
                            while (iterations < iterationsAmount && z.SquaredAbs < 4)
                            {
                                z = Complex.Pow(z, power);
                                z = Complex.Sum(z, c);
                                iterations++;
                            }

                        }

                        ComboBoxPalleteMetod(selectedPallete, x, y, iterations, iterationsAmount, bmp, z);
                    }

                    CalculatingProgress(width, x);
                }

                return bmp;
            });
            labelCalculatingProgress.Text = "рассчёт: завершён";
            SetControlsEnabled(true);
            pictureBoxFractal.Image = result;
        }

        private async void RenderGeneralizedJuliaSet()
        {
            int width = pictureBoxFractal.Width;
            int height = pictureBoxFractal.Height;
            int iterationsAmount = (int)numericUpDownIterations.Value;
            double generalYFieldChange = (double)numericUpDownGeneralizedJuliaYFieldChange.Value;
            double generalFieldSize = (double)numericUpDownGeneralizedJuliaFieldSize.Value;
            double generalXFieldChange = (double)numericUpDownGeneralizedJuliaXFieldChange.Value;
            double power = (double)numericUpDownGeneralizedJuliaPower.Value;
            double generalVariableReal = (double)numericUpDownGeneralizedJuliaCReal.Value;
            double generalVariableImage = (double)numericUpDownGeneralizedJuliaCImage.Value;
            string selectedPallete = comboBoxPallete.Text;

            double yMin = -1.5 - generalYFieldChange - generalFieldSize * 0.5;
            double yMax = 1.5 - generalYFieldChange + generalFieldSize * 0.5;
            double fieldSizeX = (yMax - yMin) * ((double)width / height);
            double xMax = 0.5 * fieldSizeX + generalXFieldChange;
            double xMin = -fieldSizeX * 0.5 + generalXFieldChange;

            SetControlsEnabled(false);

            Bitmap result = await Task.Run(() =>
            {
                Bitmap bmp = new Bitmap(width, height);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        double cReal = generalVariableReal;
                        double cImag = generalVariableImage;

                        double zReal = xMin + ((double)x / width) * (xMax - xMin);
                        double zImag = yMin + ((double)y / height) * (yMax - yMin);

                        int iterations = 0;

                        Complex z = new Complex(zReal, zImag);
                        Complex c = new Complex(cReal, cImag);

                        while (iterations < iterationsAmount && z.SquaredAbs < 4)
                        {
                            z = Complex.Pow(z, power);
                            z = Complex.Sum(z, c);
                            iterations++;
                        }

                        ComboBoxPalleteMetod(selectedPallete, x, y, iterations, iterationsAmount, bmp, z);
                    }

                    CalculatingProgress(width, x);
                }

                return bmp;
            });
            labelCalculatingProgress.Text = "рассчёт: завершён";
            SetControlsEnabled(true);
            pictureBoxFractal.Image = result;
        }

        private async void RenderBlot()
        {
            int width = pictureBoxFractal.Width;
            int height = pictureBoxFractal.Height;
            int iterationsAmount = (int)numericUpDownIterations.Value;
            double cReal = (double)numericUpDownBlotCReal.Value;
            double cImag = (double)numericUpDownBlotCImage.Value;
            string selectedPallete = comboBoxPallete.Text;

            double yMin = -1.5;
            double yMax = 1.5;
            double fieldSizeX = 3 * ((double)width / height);
            double xMax = 0.5 * fieldSizeX;
            double xMin = -fieldSizeX * 0.5;

            SetControlsEnabled(false);

            Bitmap result = await Task.Run(() =>
            {
                Bitmap bmp = new Bitmap(width, height);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        double zReal = xMin + ((double)x / width) * (xMax - xMin);
                        double zImag = yMin + ((double)y / height) * (yMax - yMin);

                        int iterations = 0;

                        Complex z = new Complex(zReal, zImag);
                        Complex c = new Complex(cReal, cImag);

                        while (iterations < iterationsAmount && z.SquaredAbs < 4)
                        {
                            z = Complex.Product(Complex.Sin(z), Complex.Square(z));
                            z = Complex.Sum(Complex.Cos(z), Complex.Pow(z, 4));
                            z = Complex.Sum(z, c);
                            iterations++;
                        }
                        ComboBoxPalleteMetod(selectedPallete, x, y, iterations, iterationsAmount, bmp, z);
                    }
                    CalculatingProgress(width, x);
                }

                return bmp;
            });
            labelCalculatingProgress.Text = "рассчёт: завершён";
            SetControlsEnabled(true);
            pictureBoxFractal.Image = result;
        }

        private async void RenderBirningShip()
        {
            int width = pictureBoxFractal.Width;
            int height = pictureBoxFractal.Height;
            int iterationsAmount = (int)numericUpDownIterations.Value;
            double generalYFieldChange = (double)numericUpDownBirningShipYFieldChange.Value;
            double generalFieldSize = (double)numericUpDownBirningShipFieldSize.Value;
            double generalXFieldChange = (double)numericUpDownBirningShipXFieldChange.Value;
            double power = (double)numericUpDownBirningShipPower.Value;
            double cReal = (double)numericUpDownBirningShipCReal.Value;
            double cImag = (double)numericUpDownBirningShipCImage.Value;
            string selectedPallete = comboBoxPallete.Text;

            double yMin = -1.5 - generalYFieldChange - generalFieldSize * 0.5;
            double yMax = 1.5 - generalYFieldChange + generalFieldSize * 0.5;
            double fieldSizeX = (yMax - yMin) * ((double)width / height);
            double xMax = 0.5 * fieldSizeX + generalXFieldChange;
            double xMin = -fieldSizeX * 0.5 + generalXFieldChange;

            SetControlsEnabled(false);

            Bitmap result = await Task.Run(() =>
            {
                Bitmap bmp = new Bitmap(width, height);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        double zReal = xMin + ((double)x / width) * (xMax - xMin);
                        double zImag = yMin + ((double)y / height) * (yMax - yMin);

                        int iterations = 0;

                        Complex z = new Complex(cReal, cImag);
                        Complex c = new Complex(zReal, zImag);

                        while (iterations < iterationsAmount && z.SquaredAbs < 4)
                        {
                            double real = Math.Abs(z.Real);
                            double image = Math.Abs(z.Image);
                            z = new Complex(real, image);
                            z = Complex.Pow(z, power);
                            z = Complex.Sum(c, z);
                            iterations++;
                        }
                        ComboBoxPalleteMetod(selectedPallete, x, y, iterations, iterationsAmount, bmp, z);
                    }
                    CalculatingProgress(width, x);
                }

                return bmp; 
            });
            labelCalculatingProgress.Text = "рассчёт: завершён";
            SetControlsEnabled(true);
            pictureBoxFractal.Image = result;
        }

        private async void RenderReversiveBirningShip()
        {
            int width = pictureBoxFractal.Width;
            int height = pictureBoxFractal.Height;
            int iterationsAmount = (int)numericUpDownIterations.Value;
            double power = (double)numericUpDownReversiveBirningShipPower.Value;
            double cReal = (double)numericUpDownReversiveBirningShipCReal.Value;
            double cImag = (double)numericUpDownReversiveBirningShipCImage.Value;
            string selectedPallete = comboBoxPallete.Text;

            double yMin = -1.5;
            double yMax = 1.5;
            double fieldSizeX = 3 * ((double)width / height);
            double xMax = 0.5 * fieldSizeX;
            double xMin = -fieldSizeX * 0.5;

            SetControlsEnabled(false);

            Bitmap result = await Task.Run(() =>
            {
                Bitmap bmp = new Bitmap(width, height);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        double zReal = xMin + ((double)x / width) * (xMax - xMin);
                        double zImag = yMin + ((double)y / height) * (yMax - yMin);

                        int iterations = 0;

                        Complex z = new Complex(zReal, zImag);
                        Complex c = new Complex(cReal, cImag);

                        while (iterations < iterationsAmount && z.SquaredAbs < 4)
                        {
                            double real = Math.Abs(z.Real);
                            double image = Math.Abs(z.Image);
                            z = new Complex(real, image);
                            z = Complex.Pow(z, power);
                            z = Complex.Sum(c, z);
                            iterations++;
                        }
                        ComboBoxPalleteMetod(selectedPallete, x, y, iterations, iterationsAmount, bmp, z);
                    }
                    CalculatingProgress(width, x);
                }

                return bmp;
            });
            labelCalculatingProgress.Text = "рассчёт: завершён";
            SetControlsEnabled(true);
            pictureBoxFractal.Image = result;
        }

        //Методы рассчёта цвета палитр
        private void WhiteBackground(int x, int y, int iterations, int iterationsAmount, Bitmap bmp)
        {
            if (iterations == iterationsAmount)
            {
                bmp.SetPixel(x, y, Color.Black);
            }
            else
            {
                bmp.SetPixel(x, y, Color.White);
            }
        }

        private void BlueGradient(int x, int y, int iterations, int iterationsAmount, Bitmap bmp)
        {
            if (iterations == iterationsAmount)
            {
                bmp.SetPixel(x, y, Color.Black);
            }
            else
            {
                int r = (int)(Math.Max(iterations - iterationsAmount / 2, 0) * (255.0 / iterationsAmount * 2));
                int g = (int)(Math.Max(iterations - iterationsAmount / 2, 0) * (255.0 / iterationsAmount * 2));
                int b = 100 + (int)(Math.Min(iterations, iterationsAmount / 2) * (155.0 / iterationsAmount * 2));
                Color _pixelColor = Color.FromArgb(r, g, b);
                bmp.SetPixel(x, y, _pixelColor);
            }
        }

        private void BlueGradientLight1(int x, int y, int iterations, int iterationsAmount, Bitmap bmp)
        {
            if (iterations == iterationsAmount)
            {
                bmp.SetPixel(x, y, Color.Black);
            }
            else
            {
                double t = Math.Sqrt((double)iterations / iterationsAmount);

                int r = (int)(Math.Max(t - 0.1, 0) * 1.2 * 255);
                int g = (int)(Math.Max(t - 0.1, 0) * 1.2 * 255);
                int b = 100 + (int)(Math.Min(t, 1.0) * 1.2 * 155);

                r = Math.Min(255, Math.Max(0, r));
                g = Math.Min(255, Math.Max(0, g));
                b = Math.Min(255, Math.Max(0, b));

                bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
            }
        }

        private void BlueGradientLight2(int x, int y, int iterations, int iterationsAmount, Bitmap bmp)
        {
            if (iterations == iterationsAmount)
            {
                bmp.SetPixel(x, y, Color.Black);
            }
            else
            {
                double t = Math.Sqrt((double)iterations / iterationsAmount);

                int r = (int)(Math.Max(t - 0.5, 0) * 3.3 * 255);
                int g = (int)(Math.Max(t - 0.5, 0) * 3.3 * 255);
                int b = 100 + (int)(Math.Min(t, 1.0) * 3.0 * 155);

                r = Math.Min(255, Math.Max(0, r));
                g = Math.Min(255, Math.Max(0, g));
                b = Math.Min(255, Math.Max(0, b));

                bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
            }
        }

        private void GraySpeedZones(int x, int y, Bitmap bmp, Complex z)
        {
            if (z.SquaredAbs < 4)
            {
                bmp.SetPixel(x, y, Color.Black);
            }
            else
            {
                int r = (int)(z.SquaredAbs / 30 * 255 * 1.6);
                int g = (int)(z.SquaredAbs / 30 * 255 * 1.6);
                int b = (int)(z.SquaredAbs / 30 * 255 * 1.6);

                r = Math.Min(255, Math.Max(0, r));
                g = Math.Min(255, Math.Max(0, g));
                b = Math.Min(255, Math.Max(0, b));

                bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
            }
        }

        private void RainbowSpeedZones(int x, int y, Bitmap bmp, Complex z)
        {
            if (z.SquaredAbs < 4)
            {
                bmp.SetPixel(x, y, Color.Black);
            }
            else
            {
                double hue = (z.Theta + Math.PI) / (Math.PI) * 180;
                double brightness = Math.Log(z.Abs) * 0.8;
                bmp.SetPixel(x, y, HsvToRgb(hue, 0.6, brightness));
            }
        }


        private void PurpleBlackSpeedZones(int x, int y, Bitmap bmp, Complex z)
        {
            if (z.SquaredAbs < 4)
            {
                bmp.SetPixel(x, y, Color.Black);
            }
            else
            {
                double brightness = (z.Theta + Math.PI) / (2 * Math.PI);
                double hue = Math.Log(3.8 + brightness) * 180;
                bmp.SetPixel(x, y, HsvToRgb(hue, 0.6, brightness));
            }
        }


        //*** метод для преобразования формата записи цвета из hsv в rgb
        private Color HsvToRgb(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            v = Math.Max(0, Math.Min(255, v));
            p = Math.Max(0, Math.Min(255, p));
            t = Math.Max(0, Math.Min(255, t));
            q = Math.Max(0, Math.Min(255, q));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }
}