using System;
using System.Collections.Generic;

namespace ComputerClub
{
    class Program
    {
        static void Main(string[] args)
        {
            ComputerClub computerClub = new ComputerClub(8);
            computerClub.Work();
        }
    }

    class ComputerClub
    {
        private int _money = 0;
        private List<Computer> _computers = new List<Computer>();
        private Queue<Schoolboy> _schoolboys = new Queue<Schoolboy>();

        public ComputerClub(int computerCount)
        {
            Random random = new Random();

            for (int i = 0; i < computerCount; i++)
            {
                _computers.Add(new Computer(random.Next(5, 15)));
            }

            CreateNewSchoolboy(25);
        }

        public void CreateNewSchoolboy(int count)
        {
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                _schoolboys.Enqueue(new Schoolboy(random.Next(100, 250), random));
            }
        }

        public void Work()
        {
            while (_schoolboys.Count > 0)
            {
                Console.WriteLine($"Computer club have - {_money} dollars $. Waiting for new client.");
                Schoolboy schoolboy = _schoolboys.Dequeue();
                Console.WriteLine($"New client in the queue, he wants bought {schoolboy.DesiredMinutes} minutes.");
                Console.WriteLine("Computers list:");
                ShowAllComputers();

                Console.WriteLine("\nYou shared him choose PC number -");
                int computerNumber = Convert.ToInt32(Console.ReadLine());

                if(computerNumber >= 0 && computerNumber < _computers.Count)
                {
                    if (_computers[computerNumber].IsBusy)
                    {
                        Console.WriteLine("You choose PC that have already choosen. Client skipped.");
                    }
                    else
                    {
                        if (schoolboy.CheckSolvency(_computers[computerNumber]))
                        {
                            Console.WriteLine("Client payed. Computer is busy for some minutes.");
                            _money += schoolboy.ToPay();

                            _computers[computerNumber].TakePlace(schoolboy);
                        }
                        else
                        {
                            Console.WriteLine("Client didn't have enough money. Client skipped.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("You didn't now what PC you must choose. Client skipped.");
                }

                Console.WriteLine("Press any key, to continue. АБОБА");
                Console.ReadKey();
                Console.Clear();
                SpendMinutes();
            }
        }

        public void SpendMinutes()
        {
            foreach(Computer computer in _computers)
            {
                computer.SpendMinute();
            }
        }

        private void ShowAllComputers()
        {
            for (int i = 0; i < _computers.Count; i++)
            {
                Console.Write($"{i} - ");
                _computers[i].ShowInfo();
            }
        }
    }


    class Computer
    {
        private Schoolboy _schoolBoy;
        private int _minutesLeft;

        public int PriceForMinutes { get; private set; }
        public bool IsBusy
        {
            get
            {
                return _minutesLeft > 0;
            }
        }

        public Computer(int priceForMinute)
        {
            PriceForMinutes = priceForMinute;
        }

        public void TakePlace(Schoolboy schoolBoy)
        {
            _schoolBoy = schoolBoy;
            _minutesLeft = schoolBoy.DesiredMinutes;
        }

        public void FreePlace()
        {
            _schoolBoy = null;
        }

        public void SpendMinute()
        {
            _minutesLeft--;
        }

        public void ShowInfo()
        {
            if (IsBusy)
            {
                Console.WriteLine($"Computer is busy. {_minutesLeft} minutes remain.");
            } 
            else
            {
                Console.WriteLine($"Computer is empty. Price for minute - {PriceForMinutes}.");
            } 
        }
    }

    class Schoolboy
    {
        private int _money;
        private int _moneyToPay;

        public int DesiredMinutes { get; private set; }

        public Schoolboy(int money, Random random)
        {
            _money = money;
            DesiredMinutes = random.Next(10, 30);
        }

        public bool CheckSolvency(Computer computer)
        {
            _moneyToPay = computer.PriceForMinutes * DesiredMinutes;
            if(_money >= _moneyToPay)
            {
                return true;
            }
            else
            {
                _moneyToPay = 0;
                return false;
            }
        }

        public int ToPay()
        {
            _money -= _moneyToPay;
            return _moneyToPay;
        }
    }
}
