using MiniReportsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniReportsProject.ViewModel
{
    public class SchoolGradeViewModel
    {
        public SchoolModel School { get; set; }
        public List<int> SelectedGrades { get; set; }
    }
}