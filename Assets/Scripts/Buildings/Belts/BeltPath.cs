using System.Collections.Generic;
using UnityEngine;

namespace Buildings.Belts
{
    public class BeltPath
    {
        public List<BeltController> Belts;
        public bool IsLoop;
        public BeltPath PathAtEnd;
        public bool LeadsToLoop => PathAtEnd != null;

        public BeltPath()
        {
            Belts = new List<BeltController>();
        }

        public void AddBelt(BeltController belt)
        {
            Belts.Insert(0, belt);
            RefreshOrder();
        }

        public BeltController GetFristBelt()
        {
            List<BeltController> tmpBelts = new List<BeltController>(Belts);

            for (int i = 0; i < Belts.Count; i++)
            {
                BeltController belt = Belts[i];

                if (!belt.TryGetItemAtOrientation(out PlaceableItemController output)) continue;
                if (output is not BeltController next || !Belts.Contains(next)) continue;

                tmpBelts.Remove(next);
            }

            if (tmpBelts.Count == 0)
            {
                Debug.LogError("Path is invalid! No first belt was found.");
                return Belts[0];
            }

            return tmpBelts[0];
        }

        public BeltController GetLastBelt()
        {
            List<BeltController> tmpBelts = new List<BeltController>(Belts);

            BeltController ret = tmpBelts[0];
            tmpBelts.RemoveAt(0);

            while (tmpBelts.Count > 0)
            {
                if (!ret.TryGetItemAtOrientation(out PlaceableItemController output)) break;

                if (output is not BeltController next || !tmpBelts.Contains(next)) break;

                tmpBelts.Remove(next);
                ret = next;
            }

            return ret;
        }

        public void MergePathWith(BeltPath other)
        {
            foreach (var belt in other.Belts)
            {
                Belts.Add(belt);
            }
        }

        public void ExecuteTransport()
        {
            for (int i = Belts.Count - 1; i >= 0; i--)
            {
                Belts[i].ExecuteTransport();
            }
        }

        public bool ContainsBelt(BeltController belt)
        {
            return Belts.Contains(belt);
        }

        void RefreshOrder()
        {
            List<BeltController> newOrder = new List<BeltController>();

            newOrder.Add(GetFristBelt());
            BeltController belt = newOrder[0];

            while (belt != null)
            {
                if (!belt.TryGetItemAtOrientation(out PlaceableItemController output)) break;
                if (output is not BeltController next || !Belts.Contains(next)) break;

                newOrder.Add(next);
                belt = next;
            }

            if (Belts.Count != newOrder.Count)
            {
                Debug.LogError("Belts.Count != newOrder.Count \t " + Belts.Count + " != " + newOrder.Count);
                string errorString = "Belts: ";
                foreach (BeltController b in Belts) errorString += b.GetIndexOnGrid() + "; ";
                errorString += "\nnewOrder: ";
                foreach (BeltController b in newOrder) errorString += b.GetIndexOnGrid() + "; ";
                Debug.LogError(errorString);
            }
        }
    }
}