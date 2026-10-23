using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    public class PathFinderVisitor : IVertexVisitor
    {
        public List<int> Path {  get; }
        public bool Found { get; private set; }
        private int Target;
        public PathFinderVisitor(int target) {  
            Target = target;
            Path = new List<int>();
            Found = false;
        }
        public void PreVisit(int vertex)
        {
            if (Found) return;
            Path.Add(vertex);
            if(vertex == Target) Found = true;

        }
        public void PostVisit(int vertex)
        {
            if(Found) return;
            if(Path.Count > 0)
            {
                Path.RemoveAt(Path.Count - 1);
            }
        }
        public override string ToString() {
            if (!Found) return "not found\n";
            return string.Join(" --> ", Path);
        }
    }
}
