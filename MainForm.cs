using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Snake.Snake;

namespace Snake
{
    public partial class MainForm : Form
    {
        public const uint fieldSize = 20;
        private const int GridSquareSize = 30;
        private const int updateDelay = GridSquareSize * 5;

        private Snake.Snake snake = new Snake.Snake(fieldSize);
        private Food food;
        private Snake.Diractions? newDirection;

        public MainForm()
        {
            InitializeComponent();
            ClientSize = new Size((int)fieldSize * GridSquareSize, (int)fieldSize * GridSquareSize);
            Console.WriteLine(this.DoubleBuffered);
            this.DoubleBuffered = true;
            MainPanel.BackColor = Color.Black;
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
            MainPanel.SuspendLayout();
            RepaintTimer.Enabled = false;
            Graphics g = e.Graphics;
            
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            
            g.FillRectangle(new SolidBrush(Color.Red), new RectangleF(food.x * GridSquareSize, food.y * GridSquareSize, GridSquareSize, GridSquareSize));

            g.FillRectangle(new SolidBrush(Color.Blue), new RectangleF(snake.body[0].x * GridSquareSize, snake.body[0].y * GridSquareSize, GridSquareSize, GridSquareSize));
            for (int i = 1; i < snake.body.Count; i++)
            {
                g.FillRectangle(new SolidBrush(Color.Green), new RectangleF(snake.body[i].x * GridSquareSize, snake.body[i].y * GridSquareSize, GridSquareSize, GridSquareSize));
            }

            MainPanel.ResumeLayout();
            RepaintTimer.Enabled = true;
        }

        private void GameCycle()
        {
            if (snake.AteAllFood)
                CreateFood();
            bool gameFinished = !snake.Update(fieldSize, food,ref newDirection);
            if (gameFinished)
            {
                Console.WriteLine(snake.Alive ? "WON" : "LOST");
                GameTimer.Stop();
            }
        }

        private void CreateFood()
        {
            Random random = new Random(System.DateTime.Now.Millisecond);

            do
                food = new Food(random.Next((int)fieldSize), random.Next((int)fieldSize));
            while (snake == food);
            snake.AteAllFood = false;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    newDirection = Diractions.down;
                    break;
                case Keys.Up:
                    newDirection = Diractions.up;
                    break;
                case Keys.Left:
                    newDirection = Diractions.left;
                    break;
                case Keys.Right:
                    newDirection = Diractions.right;
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GameCycle();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            GameTimer.Interval = updateDelay;
            GameTimer.Start();
            RepaintTimer.Interval = updateDelay;
            RepaintTimer.Start();
        }

        private void RepaintTimer_Tick(object sender, EventArgs e)
        {
             Refresh();
        }
    }
}
