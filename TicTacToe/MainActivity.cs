using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using AlertDialog = Android.App.AlertDialog;

namespace TicTacToe
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button[,] buttons;

        private int clickedButtonsCounter = 0;

        enum OX
        {
            none = 0,
            O = 1,
            X = 2
        };

        private OX current;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            buttons = new Button[,] { { FindViewById<Button>(Resource.Id.button00),
                                        FindViewById<Button>(Resource.Id.button01),
                                        FindViewById<Button>(Resource.Id.button02) },
                                      { FindViewById<Button>(Resource.Id.button10),
                                        FindViewById<Button>(Resource.Id.button11),
                                        FindViewById<Button>(Resource.Id.button12) },
                                      { FindViewById<Button>(Resource.Id.button20),
                                        FindViewById<Button>(Resource.Id.button21),
                                        FindViewById<Button>(Resource.Id.button22) } };

            foreach (var button in buttons)
            {
                button.Click += onButtonClick;
            }

            current = OX.O;

            FindViewById<Button>(Resource.Id.restartButton).Click += onResetButtonClick;
        }

        async void onButtonClick(object sender, EventArgs eventArgs)
        {
            var button = (Button)sender;
            //Toast.MakeText(this, "Clicked " + button.Id, ToastLength.Short).Show();

            if (current == OX.O)
            {
                button.SetBackgroundResource(Resource.Drawable.circle);
                button.Text = "O";
                current = OX.X;
            }
            else
            {
                button.SetBackgroundResource(Resource.Drawable.cross);
                button.Text = "X";
                current = OX.O;
            }
            
            button.Clickable = false;

            clickedButtonsCounter++;

            checkIfWon();
        }

        void checkIfWon()
        {
            string winner = "";
            //check rows
            for (var i = 0; i < 3; i++)
            {
                if (buttons[i, 0].Text != "" && buttons[i, 0].Text == buttons[i, 1].Text && buttons[i, 1].Text == buttons[i, 2].Text)
                {
                    winner = buttons[i, 0].Text;
                }
            }

            //check columns
            for (var i = 0; i < 3; i++)
            {
                if (buttons[0, i].Text != "" && buttons[0, i].Text == buttons[1, i].Text && buttons[1, i].Text == buttons[2, i].Text)
                {
                    winner = buttons[0, i].Text;
                }
            }

            //check axes
            if (buttons[0, 0].Text != "" && buttons[0, 0].Text == buttons[1, 1].Text && buttons[1, 1].Text == buttons[2, 2].Text)
            {
                winner = buttons[0, 0].Text;
            }
            if (buttons[0, 2].Text != "" && buttons[0, 2].Text == buttons[1, 1].Text && buttons[1, 1].Text == buttons[2, 0].Text)
            {
                winner = buttons[0, 2].Text;
            }

            //notify about winner
            if (winner != "")
            {
                wonAlert(winner);
            }
            else if (clickedButtonsCounter == 9)
            {
                winner = "Nobody";
                wonAlert(winner);
            }
        }

        void wonAlert(string winner)
        {
            string title = "Oops!";
            if (winner == "O")
            {
                title = "Congratulations!";
                winner = "Circle";
            }
            else if (winner == "X")
            {
                title = "Congratulations!";
                winner = "Cross";
            }

            foreach (var button in buttons)
            {
                button.Clickable = false;
            }

            string message = winner + " is winner";

            //Toast.MakeText(this, winner + " won!", ToastLength.Long).Show();

            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetPositiveButton("Ok", (senderAlert, args) => {
                Toast.MakeText(this, "Start new game", ToastLength.Short).Show();
            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        async void onResetButtonClick(object sender, EventArgs eventArgs)
        {
            //Toast.MakeText(this, "New game", ToastLength.Short).Show();

            clickedButtonsCounter = 0;

            foreach (var button in buttons)
            {
                button.SetBackgroundResource(Resource.Drawable.abc_btn_default_mtrl_shape);
                button.Text = "";
                button.Clickable = true;
            }
        }
    }
}