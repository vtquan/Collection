// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

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

            employees.Add(new Employee("Guy Walkes", "Media Relations Specialist", "Guy.Walkes@microsoft.com", new DateTime(2011, 12, 13)));
            employees.Add(new Employee("Neil Striegel", "Crime Scene Technician", "Neil.Striegel@microsoft.com", new DateTime(2002, 1, 31)));
            employees.Add(new Employee("Ted Beverlin", "Air Antisubmarine Officer", "Ted.Beverlin@microsoft.com", new DateTime(2012, 11, 09)));
            employees.Add(new Employee("Tyrone Brinkerhoff", "Ringmaster", "Tyrone.Brinkerhoff@microsoft.com", new DateTime(1903, 5, 06)));
            employees.Add(new Employee("Jamie Sharon", "Releaser of the Hounds", "Jamie.Sharon@microsoft.com", new DateTime(1999, 11, 09)));
            employees.Add(new Employee("Kurt Swinderman", "Mime", "Kurt.Swinderman@microsoft.com", new DateTime(2010, 6, 15)));
            employees.Add(new Employee("Mathew Schwartzberg", "Bird Trapper", "Mathew.Schwartzberg@microsoft.com", new DateTime(2012, 12, 26)));
            employees.Add(new Employee("Selena Borelli", "Carny", "Selena.Borelli@microsoft.com", new DateTime(2002, 1, 31)));
            employees.Add(new Employee("Dona Londono", "Cowpuncher", "Dona.Londono@microsoft.com", new DateTime(2012, 11, 09)));
            employees.Add(new Employee("Carmella Polito", "Door-to-Door Salesman", "Carmella.Polito@microsoft.com", new DateTime(1903, 5, 06)));
            employees.Add(new Employee("Kelly Ress", "Hobo", "Kelly.Ress@microsoft.com", new DateTime(1999, 11, 09)));
            employees.Add(new Employee("Julio Reinoso", "Swishy Hairdresser", "Julio.Reinoso@microsoft.com", new DateTime(2010, 6, 15)));
            employees.Add(new Employee("Darcy Gammel", "Pickpocket", "Darcy.Gammel@microsoft.com", new DateTime(2012, 12, 26)));
            employees.Add(new Employee("Melisa Patricia", "Extreme AC Repairman", "Melisa.Patricia@microsoft.com", new DateTime(2002, 2, 6)));
            employees.Add(new Employee("Milagros Asmussen", "Crime Fighter", "Milagros.Asmussen@microsoft.com", new DateTime(2004, 1, 2)));

            return employees;
        }
    }
}
