using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Ships
{
    [Serializable]
    public class Crew
    {
        public float health = 100, maxHealth = 100;
        public float scurvy = 0;  // 0 is scurvy-free, 0.5 is bordering, and 1 is scurvy
        public float hunger = 0; // 0 is full & healing, 0.5 is "I could grab a bite", and 1 is starving
        public int crew = 10, maxCrew = 10;

        public DateTime lastUpdated = DateTime.Now;

        public Crew() {

        }

        public void FeedOranges(int num)
        {
            scurvy -= (num / crew) / 10f;
            if (scurvy < 0) scurvy = 0;

            hunger -= (num / crew) / 10f;
            if (hunger < 0) hunger = 0;
        }

        public void UpdateCrewHealth()
        {
            float delta = (lastUpdated - DateTime.Now).Ticks;
            lastUpdated = DateTime.Now;

            health -= (scurvy * delta) / 100;
            scurvy += 0.001f * delta;

            health -= ((hunger - 0.5f) * delta) / 100;
            hunger += 0.001f * delta;
        }

        // Calculates the crew's performance based on their wellness and quantity
        public float CalculatePerformance()
        {
            UpdateCrewHealth();
            return crew * health;
        }

        public Crewmate[] GenerateCrewmates()
        {
            Crewmate[] list = new Crewmate[crew];
            return list;
        }
    }
}
