using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
namespace Assets.Scritps
{
    public class DPSClass
    {
        public DPSClass()
        {
            dpsQue = new Queue<damageTracker>();
        }

        float latestTimestamp;

        struct damageTracker
        {
            public damageTracker(float ts, int d)
            {
                timestamp = ts;
                damage = d;
            }
            
            public float timestamp;
            public int damage;
        }


        // Most recent damage is going to at the top of the queue.
        // If the timestamp for the item at the bottom of the queue is more than one second older than the item
        // at the top of the queue, then dequeue it. 
        // DPS is calculated by summing all the items in the queue. 
        private Queue<damageTracker> dpsQue;

        public void addDamage(int damage)
        {
            float timestamp = UnityEngine.Time.time;
            damageTracker damageItem = new damageTracker(timestamp, damage);
            latestTimestamp = timestamp;
            dpsQue.Enqueue(damageItem);

        }

        public float getDPS()
        {
            if (dpsQue.Count() == 0) return 0;
            while (dpsQue.Peek().timestamp < latestTimestamp-1.0)
            {
                dpsQue.Dequeue();
            }
            float total = 0f;
            foreach (damageTracker d in dpsQue)
            {
                total += d.damage;
            }
            return total;
        }

    }
}
