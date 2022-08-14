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
                        Mathf.Pow(postion.x, 3f),
                        Mathf.Pow(postion.y, 3f),
                        Mathf.Pow(postion.z, 3f)
                        );

                newSmoothedPosition /= deepOfRemember;

                newSmoothedPosition = new Vector3(
                    Mathf.Pow(newSmoothedPosition.x, 1f / 3f),
                    Mathf.Pow(newSmoothedPosition.y, 1f / 3f),
                    Mathf.Pow(newSmoothedPosition.z, 1f / 3f)
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
