using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures.Matrices;

namespace MachineLearning
{
    public class SvmClassifier
    {
        public virtual void Train(Vector<double>[] trainingData, string[] classLabels)
        {
        }
    }

    public class LinearSvmClassifier : SvmClassifier
    {
        public override void Train(Vector<double>[] trainingData, string[] classLabels)
        {
            var classes = classLabels.Distinct().ToArray();
            if (classes.Length > 2)
            {
                throw new NotSupportedException("Multiclass SVMs have not yet been implemented. For now use only 2 classes.");
            }
            Dictionary<string, List<Vector<double>>> partitioned = classes.ToDictionary(classLabel => classLabel, classLabel => new List<Vector<double>>());
            for (int i = 0; i < trainingData.Length; i++)
            {
                var vector = trainingData[i];
                var classLabel = classLabels[i];
                partitioned[classLabel].Add(vector);
            }
            var classOneItems = partitioned[classLabels[0]];
            var classTwoItems = partitioned[classLabels[1]];
            Vector<double> classOneSv;
            Vector<double> classTwoSv;
            double smallestDistance = double.NaN;
            foreach (Vector<double> classOneItem in classOneItems)
            {
                foreach (Vector<double> classTwoItem in classTwoItems)
                {
                    double distance = classOneItem.Distance(classTwoItem);
                    if (double.IsNaN(smallestDistance) || distance < smallestDistance)
                    {
                        smallestDistance = distance;
                        classOneSv = classOneItem;
                        classTwoSv = classTwoItem;
                    }
                }
            }
        }
    }
}
