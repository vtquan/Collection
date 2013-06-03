using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEmployees
{
    class Employee : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set 
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set 
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        private DateTime _hired;
        public DateTime Hired
        {
            get { return _hired; }
            set
            {
                _hired = value;
                OnPropertyChanged("Hired");
            }
        }

        public Employee(string name, string title, string email, DateTime hired)
        {
            this.Name = name;
            this.Title = title;
            this.Email = email;
            this.Hired = hired;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public static ObservableCollection<Employee> GetEmployees()
        {
            var employees = new ObservableCollection<Employee>();

            employees.Add(new Employee("Person One", "Job1", "faker1@cool.com", new DateTime(2011, 12, 26)));
            employees.Add(new Employee("Person Two", "Job2", "faker2@cool.com", new DateTime(2012, 12, 26)));
            employees.Add(new Employee("Person Three", "Job3", "faker3@cool.com", new DateTime(2013, 12, 26)));
            employees.Add(new Employee("Person Four", "Job4", "faker4@cool.com", new DateTime(2014, 12, 26)));

            return employees;
        }
    }
}
