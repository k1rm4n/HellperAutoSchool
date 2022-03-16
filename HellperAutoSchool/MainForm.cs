using System;
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

        private void ComboListTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            string comboBoxState = comboListTable.Items[comboListTable.SelectedIndex].ToString();

            if (comboBoxState == "Студенты")
            {
                student.AddTableStudents(this);
                instructor.RemoveTableInstructors(this);
                teacher.RemoveTableTeachers(this);
                group.RemoveTableGroups(this);
                car.RemoveTableCars(this);
                classTime.RemoveTableClassTime(this);
            }
            else if (comboBoxState == "Учителя")
            {
                teacher.AddTableTeachers(this);
                student.RemoveTableStudents(this);
                instructor.RemoveTableInstructors(this);
                group.RemoveTableGroups(this);
                car.RemoveTableCars(this);
                classTime.RemoveTableClassTime(this);
            }
            else if (comboBoxState == "Инструкторы")
            {
                instructor.AddTableInstructors(this);
                teacher.RemoveTableTeachers(this);
                student.RemoveTableStudents(this);
                group.RemoveTableGroups(this);
                car.RemoveTableCars(this);
                classTime.RemoveTableClassTime(this);
            }
            else if (comboBoxState == "Автомобили")
            {
                car.AddTableCars(this);
                instructor.RemoveTableInstructors(this);
                teacher.RemoveTableTeachers(this);
                student.RemoveTableStudents(this);
                group.RemoveTableGroups(this);
                classTime.RemoveTableClassTime(this);
            }
            else if (comboBoxState == "Группы")
            {
                group.AddTableGroups(this);
                car.RemoveTableCars(this);
                instructor.RemoveTableInstructors(this);
                teacher.RemoveTableTeachers(this);
                student.RemoveTableStudents(this);
                classTime.RemoveTableClassTime(this);
            }
            else if (comboBoxState == "Расписание")
            {
                classTime.AddTableClassTime(this);
                group.RemoveTableGroups(this);
                car.RemoveTableCars(this);
                instructor.RemoveTableInstructors(this);
                teacher.RemoveTableTeachers(this);
                student.RemoveTableStudents(this);
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Hide();
            Autorization auto = new Autorization();
            auto.Show();
        }
    }
}
