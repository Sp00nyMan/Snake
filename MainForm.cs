using System;
using System.Drawing;
using System.Windows.Forms;
using Snake.Snake;

namespace Snake
{
    public partial class MainForm : Form
    {
        public static readonly uint FieldSize = 20;
        private static readonly int GridSquareSize = 30;
        private static readonly int updateDelay = GridSquareSize * 3;

        private Snake.Snake snake = new Snake.Snake(FieldSize);
        private Food food;
        private Snake.Diractions? newDirection;

        public MainForm()
        {
            InitializeComponent();
            ClientSize = new Size((int)FieldSize * GridSquareSize, (int)FieldSize * GridSquareSize);

            StartPosition = FormStartPosition.WindowsDefaultLocation;
           
            MainPanel.BackColor = Color.Black;
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
/*            for (int i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    g.DrawRectangle(new Pen(Color.White),
                        new Rectangle(i * GridSquareSize, j * GridSquareSize, GridSquareSize, GridSquareSize));
                }
            }
*/
            g.FillRectangle(new SolidBrush(Color.Red), new RectangleF(food.x * GridSquareSize, food.y * GridSquareSize, GridSquareSize, GridSquareSize));

            for (int i = snake.body.Count - 1; i > 0; i--)
            {
                g.FillRectangle(new SolidBrush(Color.Green), new RectangleF(snake.body[i].x * GridSquareSize, snake.body[i].y * GridSquareSize, GridSquareSize, GridSquareSize));
            }
            g.FillRectangle(new SolidBrush(Color.Blue), new RectangleF(snake.body[0].x * GridSquareSize, snake.body[0].y * GridSquareSize, GridSquareSize, GridSquareSize));
        }

        private void GameCycle()
        {
            if (snake.AteAllFood)
                CreateFood();
            bool gameFinished = !snake.Update(FieldSize, food,ref newDirection);
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
                food = new Food(random.Next((int)FieldSize), random.Next((int)FieldSize));
            while (snake == food);
            snake.AteAllFood = false;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(GameTimer.Enabled)
                switch (e.KeyCode)
                {
                    case Keys.Down:
                        newDirection = Diractions.Down;
                        break;
                    case Keys.Up:
                        newDirection = Diractions.Up;
                        break;
                    case Keys.Left:
                        newDirection = Diractions.Left;
                        break;
                    case Keys.Right:
                        newDirection = Diractions.Right;
                        break;
                }
            else if (e.KeyCode == Keys.Space)
            {
                snake = new Snake.Snake(FieldSize);
                newDirection = null;
                GameTimer.Start();
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            GameCycle();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GameTimer.Interval = updateDelay;
            RepaintTimer.Interval = updateDelay;
            GameTimer.Start();
            RepaintTimer.Start();
        }

        private void RepaintTimer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
