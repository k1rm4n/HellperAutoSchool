﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HellperAutoSchool
{
    public partial class MainForm : Form
    {
        Student student = new Student();
        Instructor instructor = new Instructor();
        Teacher teacher = new Teacher();
        Group group = new Group();
        Car car = new Car();
        ClassTime classTime = new ClassTime();


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            /*student.AddTableStudents(this);*/
            /*instructor.AddTableInstructors(this);*/
            /*teacher.AddTableTeachers(this);*/
            /*group.AddTableGroups(this);*/
            /*car.AddTableCars(this);*/
            classTime.AddTableClassTime(this);

        }
    }
}
