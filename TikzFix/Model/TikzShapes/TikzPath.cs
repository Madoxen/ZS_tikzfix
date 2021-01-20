using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

using TikzFix.Model.Styling;
using TikzFix.Model.Tool;

namespace TikzFix.Model.TikzShapes
{
    class TikzPath : TikzShape
    {
        public TikzPath(Path path, TikzStyle style, string rawData) : base(path, style) {
            this.path = path;
            this.rawData = rawData;
        }

        private string rawData;
        private Path path;
        public override Shape Shape {
            get => path;
            set
            {
                if (value is not Path p)
                {
                    throw new ArgumentException("TikzLine Shape has to be Line");
                }
                path = p;
            }
        }

        public override LocalShapeData GenerateLocalData()
        {
            throw new NotImplementedException();
        }

        public override string GenerateTikz()
        {
            throw new NotImplementedException();
        }
    }
}
