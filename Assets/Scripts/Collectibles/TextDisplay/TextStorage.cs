using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextStorage : MonoBehaviour
{
    private string[] entries = new string[] {
        "For generations our kind has kept the cores aflame, so that life would persist in these harsh lands. I have tried my best to stand my ground against the corrupting cold. So few of us are left now. Soon my own heat will run out. Who will carry on our sacred duty?",
        "We were seen as protectors of the land, beloved by all of the creatures. Then the Freezing came, and the eyes that once regarded us with awe and reverence now gazed at us with animosity. Now we are a just a convenience, their means of survival against the cold. How far we have fallen.",
        "Bears… they attacked our village… everything was destroyed. Many of our brethren were dragged away, even the little ones. We have done nothing to incur this ferocity… why did this happen to us?",
        "I have never seen a leviathan of such a size... when it moves, the mighty vibrations can be felt throughout the forest. Has the cold driven it toward us? It has taken to dwelling near the Core in the forest, and aggressively attacks us on our way up the Mountain.",
        "The annual pilgrimage to the peak of the Mountain has been our long-established tradition, attempted only by the strongest of our warriors. Few had the strength to make it to the top – even fewer had the force of vitality to restore the Core there, which endured the harshest of the cold winds.",
        "Today our leader finally succumbed to the cold and lost his flame. We would have buried him with full honours on the Mountain – yet to our shame we did not even have enough heat among ourselves to make the journey.",
        "Till the end, he adamantly refused to consume the forest, staying true to the ancient agreement with the land to live on the offerings of the creatures. But those offerings have long ceased. O leader… I wish I had your level of conviction.",
        "When the Core in the Mountain broke, large amounts of mana surged rampantly through the Mountain. In rocky clefts and icy caves the mana collected, condensing into miniature mana crystals that animated the rocks and ice around them, creating the elementals.",
        "Initially, we considered them like us. However, we soon discovered they were not living beings, for they were incapable of any rationality. Mindlessly they would wander back and forth, attacking indiscriminately.",
        "He dwells above us all, gazing down on his frozen kingdom. Everything we have experienced is all this monster’s doing. As he gleefully continues to freeze the world over, the last few living creatures die, and the final Cores die out. Forgive me, for I have failed. I know it is my duty as protector, but he is too strong. And I don’t think I can stand before him again… THOSE EYES…"
    };
    

    // Start is called before the first frame update
    public string Retrieve(int i)
    {
        return entries[i];
    }

}
