using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    private bool _finishedTutorial = false;
    DialogueManager _dialogueManager;
    public bool GetFinishedTutorial()
    {
        return _finishedTutorial;
    }

    public void SetFinishedTutorial(bool FinishedTutorial)
    {
        _finishedTutorial = FinishedTutorial;
    }

    private void Start()
    {
        _dialogueManager = GameObject.FindObjectOfType<DialogueManager>();

        string[] dialogues = { "The world as we used to know it has long ago changed...",
                               "Now it's only you against whatever these creatures are...",
                               "You have to survive enough time, then you'll finally be free...",
                               "Welcome to Hell Crusaders. In this survival horror game it's you, that infinite ammunition M4 you inherited from a stranger that died in front of you, and endless of terrible creatures.",
                               "Kill them all before they kill you or you'll regret it. If you can even regret stuff when inert...",
                               "Why don't we try it out!? Let yourself be killed, then try to regret following the orders of someone you can't even see...",
                               "Then let me know what you find out! Deal? Deal!",
                               "What? You don't want to do it? Fine... have fun surviving then!",
                               "Then again, you're so pathetic that I got no choice but pity you... I'll tell you how to use that weapon of yours, at the very least...",
                               "Click your mouse's left button to shoot, use your ammunitation carefully! Sure it's infinite but you have to reload... that's precious time.",
                               "Press R if you ever want to reload, be careful though, you can't shoot while reloading! Try doing it while you're surrounded though, I bet it'll be fun as hell!",
                               "No? Boo, party pooper! Hold your mouse's right button and move your cursor to aim, you will rotate with your came-- ahem, your whole body will rotate around so don't get sick.",
                               "What else...? Oh, right! Use your WASD keys to move around:\nW - Forward / S - Backwards / A - Left / D - Right\nUse Left Shift to toggle Running",
                               "While running, your speed will increase considerably, but you can only move forward. Running is not a political game to play Left or Right, my dude! And running backwards doesn't sound like a good idea...",
                               "As for the enemies... I know, I know, you hate tutorials, so do I! Just stop reading and get into action, but you probably shouldn't do that...",
                               "Back to my speech... there's 5 different types of enemies: these white ones are ice enemies! Get too close and you'll be frozen! That'll slightly reduce your movement speed... ouch!",
                               "There's these too, fire wisps! This kind is specially dangerous as they deal more damage than the other ones. Kinda hot, don'tcha think?",
                                "Ooh!! The toxic ones! Very nasty... if they hit you you'll be instantly poisoned for a long time... draining 1HP per second. Pretty bad news.",
                                "And finally... the electric ones! One hit and you'll be both Shocked and Zapped! Shocked only lasts 1 second, but it'll reduce your movement speed by 80%...",
                                "And Zapped lasts 2 seconds but it decreases your shooting speed by 20%. That's a pretty big deal for a shooting game, ain't it?",
                                "Ah! I almost forgot... my favorite ones... Capuchas! A strange name for a strange kind... these dudes behave differently, they will target other enemies to...",
                                "Heal them! Pretty gross! Don't let them live too long or they'll make a mess out of your gameplay! As a reminder, they don't only heal, they also apply a shield to enemies...",
                                "The longer it casts its magic to other enemies, the tougher it'll become to deal with them. Try to focus on them before they become your nightmare!",
                                "I'll even make it easier on you, some of them are pretty dumb and will target you instead. They won't heal you but they won't damage you either, they're kinda just annoying...",
                                "...but let them accumulate around you and they'll become a meat shield for other important targets... that's a no-no in my book.",
                                "The amount of enemies will increase in sheer numbers per round. Will you last 'till Round 10? I bet you won't.",
                                "To make it at least less-pitiful for a pitiful 3D mode- ahem, person! Yeah, for a pitiful person like you, I'll summon a Medical Kit for you at the end of each round.",
                                "They don't despawn so pick them whenever you want, but they spawn in the same place so don't waste them either. Pick all or pick none, my grandma would say.",
                                "Yeah! I have a grandma too! What do you think I am? A sadist that enjoys watching you play this while I keep making this tutorial uneccesarily long!? Pfft, nonsense!",
                                "A Medical Kit heals you by 50 HP so that's really strong considering that's half your health. Pfft, as if you could stay alive for longer than 5 seconds anyway.",
                                "Anyway! Too long of an introduction amirite? Let's continue, the big babies gotta grow and be ruthlessly murdered by my minions-- ahem, I mean, by those evil creatures nobody knows who they serve!",
                                "Welcome to your new adventure! Click on Leave and the beginning of your end will start! Curtains up!"
        };
        _dialogueManager.SetDialogue("Unknown Entity", dialogues);
    }
}
