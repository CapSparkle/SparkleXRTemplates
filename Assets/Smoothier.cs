using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SparkleXRTemplates
{
    public enum SmoothMethod
    {
        Mean,
        RootMean,
    }

    public class PositionSmoothier
    {
        //number of last values to contain
        int deepOfRemember = 10;
        int historyStoreIndexer;
        SmoothMethod smoothMethod = SmoothMethod.Mean;

        Vector3[] positionHistory;
        public Vector3 smoothedPosition { get; private set; }


        public PositionSmoothier(int deepOfRemember = 10, SmoothMethod smoothMethod = SmoothMethod.Mean)
        {
            this.deepOfRemember = deepOfRemember;
            historyStoreIndexer = 0;
            positionHistory = new Vector3[this.deepOfRemember];

            this.smoothMethod = smoothMethod;
        }
        void StoreValue(Vector3 newPosition)
        {
            positionHistory[historyStoreIndexer] = newPosition;
            incIndexer();
        }

        void incIndexer()
        {
            historyStoreIndexer++;
            historyStoreIndexer %= deepOfRemember;
        }

        void CalculateSmoothedPosition()
        {
            Vector3 newSmoothedPosition = Vector3.zero;

            if (smoothMethod == SmoothMethod.Mean)
            {
                foreach (Vector3 postion in positionHistory)
                    newSmoothedPosition += postion;


                newSmoothedPosition /= deepOfRemember;

            }
            else if (smoothMethod == SmoothMethod.RootMean)
            {
                foreach (Vector3 postion in positionHistory)
                    newSmoothedPosition += new Vector3(
                        Mathf.Pow(postion.x, 2f),
                        Mathf.Pow(postion.y, 2f),
                        Mathf.Pow(postion.z, 2f)
                        );

                newSmoothedPosition /= deepOfRemember;

                newSmoothedPosition = new Vector3(
                    Mathf.Sqrt(newSmoothedPosition.x),
                    Mathf.Sqrt(newSmoothedPosition.y),
                    Mathf.Sqrt(newSmoothedPosition.z)
                    );
            }

            smoothedPosition = newSmoothedPosition;
        }

        public Vector3 PerformSmoothing(Vector3 newPosition)
        {
            if (newPosition != Vector3.zero)
                StoreValue(newPosition);

            CalculateSmoothedPosition();

            return smoothedPosition;
        }
    }
}
