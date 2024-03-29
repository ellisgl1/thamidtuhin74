﻿using Rubyer.Commons;
using Rubyer.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RubyerDemo.ViewModels
{
    public class ListsViewModel : ViewModelBase
    {
        public ListsViewModel()
        {
            Persons = new ObservableCollection<Person>
            {
                new Person{ Id=1,Name="张三",Age=17,IsSelected=false,Gender=GenderType.Men},
                new Person{ Id=2,Name="李四",Age=18,IsSelected=true,Gender=GenderType.Men},
                new Person{ Id=3,Name="王五",Age=19,IsSelected=false,Gender=GenderType.Women},
                new Person{ Id=4,Name="赵六",Age=20,IsSelected=true,Gender=GenderType.Men},
                new Person{ Id=5,Name="孙七",Age=21,IsSelected=false,Gender=GenderType.Men},
                new Person{ Id=6,Name="周八",Age=22,IsSelected=true,Gender=GenderType.Women},
                new Person{ Id=7,Name="吴九",Age=23,IsSelected=false,Gender=GenderType.Men}
            };

            Catalogs = new ObservableCollection<Catalog>
            {
                new Catalog
                {
                    Name = "乐器",
                    Items = new ObservableCollection<Catalog>
                    {
                        new Catalog{ Name = "钢琴" },
                        new Catalog
                        {
                            Name = "吉他",
                            Items = new ObservableCollection<Catalog>
                            {
                            new Catalog{ Name = "木吉他"},
                            new Catalog{ Name = "电吉他"}
                            }
                        },
                        new Catalog{ Name = "二胡" }
                    }
                },
                new Catalog
                {
                    Name = "运动",
                    Items = new ObservableCollection<Catalog>
                    {
                        new Catalog{ Name = "跑步" },
                        new Catalog{ Name = "篮球" },
                        new Catalog{ Name = "跑步" },
                    }
                }
            };
        }

        private ObservableCollection<Person> persons;

        public ObservableCollection<Person> Persons
        {
            get => persons;
            set
            {
                persons = value;
                RaisePropertyChanged("Persons");
            }
        }

        private ObservableCollection<Catalog> catalogs;

        public ObservableCollection<Catalog> Catalogs
        {
            get => catalogs;
            set
            {
                catalogs = value;
                RaisePropertyChanged("Catalogs");
            }
        }

        private FoodType currentFood;

        public FoodType CurrentFood
        {
            get => currentFood;
            set
            {
                currentFood = value;
                RaisePropertyChanged("CurrentFood");
            }
        }
    }

    public class Person : NotifyPropertyObject
    {
        private int id;

        [Display(Name = "序号", AutoGenerateField = false)]
        public int Id
        {
            get => id;
            set
            {
                id = value;
                RaisePropertyChanged("Id");
            }
        }

        private string name;

        [Display(Name = "名称")]
        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private int age;

        [Display(Name = "年龄")]
        [DisplayFormat(DataFormatString = "{0}岁")]
        public int Age
        {
            get => age;
            set
            {
                age = value;
                RaisePropertyChanged("Age");
            }
        }

        private bool isSelected;

        [Display(Name = "选择")]
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        private GenderType gender;

        [Display(Name = "性别")]
        public GenderType Gender
        {
            get => gender;
            set
            {
                gender = value;
                RaisePropertyChanged("Gender");
            }
        }
    }

    public class Catalog
    {
        public string Name { get; set; }
        public ObservableCollection<Catalog> Items { get; set; }
    }

    /// <summary>
    /// 性别
    /// </summary>
    [TypeConverter(typeof(EnumDescriptionConverter))]
    public enum GenderType
    {
        [Description("男")]
        Men,

        [Description("女")]
        Women
    }
}