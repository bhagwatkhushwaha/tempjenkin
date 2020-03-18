using System;
using System.Collections.Generic;
using System.Text;

namespace Autumn.AutumnUser.Dto
{
    public class GraphDataDto
    {
        public int CurrentAge { get; set; }

        public int LifeExpectancy { get; set; }

        public double DesiredLegacyAmount { get; set; }

        public List<int> RetirementAges { get; set; }

        public double DesRetSum { get; set; }

        public List<double> LikRetSums { get; set; }

        public List<double> LikRetLegacies { get; set; }
    }
}
