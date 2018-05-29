using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftDialogue;
using VisualRPG;

public class DialogueExample : MonoBehaviour {

    public DialogueContext dialogueContext;
    public CombatUI combatUI;
    public VisualCombatUI visualCombatUI;

    public Character akarys;
    public Character krynn;

    void Start() {
        GenExample();

        akarys = Character.GetAkarys();
        krynn = Character.GetKrynn();

        //Debug.Log(akarys.attack.GetSpellDialogue(akarys, krynn));
        //Debug.Log(krynn.attack.GetSpellDialogue(krynn, akarys));
        //Debug.Log(akarys.spell2.ApplyEffect(akarys, akarys));
        //Debug.Log(krynn.spell2.GetSpellDialogue(krynn, akarys));
        //Debug.Log(akarys.spell1.GetSpellDialogue(akarys, krynn));
        //Debug.Log(krynn.spell1.GetSpellDialogue(krynn, akarys));
        //Debug.Log(akarys.attack.GetSpellDialogue(akarys, krynn));
        //Debug.Log(krynn.attack.GetSpellDialogue(krynn, akarys));

        //Debug.Log("Name: " + akarys.name);
        //Debug.Log("Strength: " + akarys.attributes.GetMainAttributeValue(MainAttribute.Strength));
        //Debug.Log("Physical: " + akarys.attributes.GetSecondaryAttributeValue(SecondaryAttribute.PhysicalDamage) + "/" + akarys.attributes.GetSecondaryAttributeValue(SecondaryAttribute.PhysicalResistance));
        //Debug.Log("Spirit: " + akarys.attributes.GetMainAttributeValue(MainAttribute.Spirit));
        //Debug.Log("Light: " + akarys.attributes.GetSecondaryAttributeValue(SecondaryAttribute.LightDamage) + "/" + akarys.attributes.GetSecondaryAttributeValue(SecondaryAttribute.LightResistance));

        //Debug.Log("Name: " + krynn.name);
        //Debug.Log("Strength: " + krynn.attributes.GetMainAttributeValue(MainAttribute.Strength));
        //Debug.Log("Physical: " + krynn.attributes.GetSecondaryAttributeValue(SecondaryAttribute.PhysicalDamage) + "/" + krynn.attributes.GetSecondaryAttributeValue(SecondaryAttribute.PhysicalResistance));
        //Debug.Log("Spirit: " + krynn.attributes.GetMainAttributeValue(MainAttribute.Spirit));
        //Debug.Log("Light: " + krynn.attributes.GetSecondaryAttributeValue(SecondaryAttribute.LightDamage) + "/" + krynn.attributes.GetSecondaryAttributeValue(SecondaryAttribute.LightResistance));
    }

    private void GenExample() {
        Debug.Log("GenExample");
        DialogueNode startingPoint = GenBranch0();
        GenBranch1();
        GenBranch2();
        GenChoice10();
        GenChoice11();
        //GenFight100();
        GenFight101();
        GenBranch3();
        dialogueContext.Active(startingPoint);
    }

    private DialogueNode GenBranch0(long id = 0, long next = 1) {
        DialogueBranch output = dialogueContext.CreateDialogueBranch(id, next);
        output
            .AddNode(new DialogueSpeech("(Hit [Left clic] or [Space] to progress...)")
                .AddEffect()
                .AddHandler(new DialogueEffectSetAudioClipHandler(0))
                .AddHandler(new DialogueEffectSetAudioClipSourceHandler(0, "SoftDialogueExample/Audio/Intro"))
                .AddHandler(new DialogueEffectSetAudioClipHandler(1))
                .AddHandler(new DialogueEffectSetAudioClipSourceHandler(1, "SoftDialogueExample/Audio/MainTheme"))
                .AddHandler(new DialogueEffectSetAudioClipHandler(2))
                .AddHandler(new DialogueEffectSetAudioClipSourceHandler(2, "SoftDialogueExample/Audio/BatHitMiss"))
                .AddHandler(new DialogueEffectSetSpriteHandler(3))
                .AddHandler(new DialogueEffectSetSpriteSourceHandler(3, "SoftDialogueExample/Sprites/Doge"))
                .AddHandler(new DialogueEffectSetSpriteSizeHandler(3, new Vector2(1, 1)))
                .AddHandler(new DialogueEffectSnapSpriteToPositionHandler(3, new Vector3(0, 0), SpriteSnapPosition.Bottom)))
            .AddNode(new DialogueSpeech("...")
                .AddEffect()
                .AddHandler(new DialogueEffectPlayAudioHandler(2))
                .AddHandler(new DialogueEffectSetSpriteVisibleHandler(3, true)))
            .AddNode(new DialogueSpeech("This is a sample.")
                .AddEffect()
                .AddHandler(new DialogueEffectPlayAudioHandler(2))
                .AddHandler(new DialogueEffectFlipSpriteHandler(3, true, false))
                .AddHandler(new DialogueEffectWaitHandler(0.1f))
                .AddHandler(new DialogueEffectPlayAudioHandler(2))
                .AddHandler(new DialogueEffectFlipSpriteHandler(3, false, false))
                .AddHandler(new DialogueEffectWaitHandler(0.1f))
                .AddHandler(new DialogueEffectPlayAudioHandler(2))
                .AddHandler(new DialogueEffectFlipSpriteHandler(3, true, false))
                .AddHandler(new DialogueEffectWaitHandler(0.1f))
                .AddHandler(new DialogueEffectPlayAudioHandler(2))
                .AddHandler(new DialogueEffectFlipSpriteHandler(3, false, false)))
            .AddNode(new DialogueSpeech("...")
                .AddEffect()
                .AddHandler(new DialogueEffectPlayAudioHandler(2))
                .AddHandler(new DialogueEffectScaleSpriteHandler(3, 0.3f, new Vector2(0.1f, 0.1f)))
                .AddHandler(new DialogueEffectMoveSpriteHandler(3, 0.3f, new Vector3(10, 0, 0), SpriteSnapPosition.Bottom))
                .AddHandler(new DialogueEffectWaitHandler(0.4f)))
            .AddNode(new DialogueSpeech("Let's get to it already!!!")
                .AddEffect()
                .AddHandler(new DialogueEffectPlayAudioHandler(2))
                .AddHandler(new DialogueEffectSetSpriteVisibleHandler(3, false)))
            .AddNode(new DialogueSpeech("")
                .AddEffect()
                .AddHandler(new DialogueEffectSetConditionHandler("FIRST")));

        return output;
    }


    private DialogueNode GenBranch1(long id = 1, long next = 10) {
        DialogueBranch output = dialogueContext.CreateDialogueBranch(id, next);
        output
            .AddEffect()
            .AddHandler(new DialogueEffectPlayMusicHandler(0));
        output
            .AddNode(new DialogueSpeech("The cold night wind is softly blowing. A fine rain is slowly turning the streets wet."))
            .AddNode(new DialogueSpeech("Sometimes, the bright frontlight of some car enlighten the dark concrete."))
            .AddNode(new DialogueSpeech("Nobody's walking here by that time and weather."))
            .AddNode(new DialogueSpeech("But I do. I love the peaceful ambiance."))
            .AddNode(new DialogueSpeech("The quiet environment is perfect to reflect on my recent life decisions."))
            .AddNode(new DialogueSpeech("My head is empty from the harsh thoughts that cloud my spirit during my day to day life."))
            .AddNode(new DialogueSpeech("But maybe it's not clear. Do you want me to repeat myself?"));
        return output;
    }


    private DialogueNode GenBranch2(long id = 2, long next = 1) {
        DialogueBranch output = dialogueContext.CreateDialogueBranch(id, next);
        output
            .AddNode(new DialogueSpeech("You are pretty slow now, aren't ya?"))
            .AddNode(new DialogueSpeech(""));
        return output;
    }


    private DialogueNode GenBranch3(long id = 3, long next = -1) {
        DialogueBranch output = dialogueContext.CreateDialogueBranch(id++, next);
        output
            .AddEffect()
            .AddHandler(new DialogueEffectPlayMusicHandler(0));
        output
            .AddNode(new DialogueSpeech("Thank you for reading :)", "Daratrix the weakburger"))
            .AddNode(new DialogueSpeech("(The next click will end the game...)"));
        return output;
    }

    private DialogueNode GenChoice10(long id = 10, long id1 = 11, long id2 = 101) {
        DialogueActiveChoice output = dialogueContext.CreateDialogueActiveChoice(id);
        output
            .AddOption(new DialogueActiveOption(id1, "Read again"))
            .AddOption(new DialogueActiveOption(id2, "Fuck this shit, I'm out!"));
        return output;
    }

    private DialogueNode GenChoice11(long id = 11, long id1 = 2, long id2 = 1) {
        DialoguePassiveChoice output = dialogueContext.CreateDialoguePassiveChoice(id);
        output
            .AddOption(new DialoguePassiveOption(id1, new string[] { "!FIRST" }))
            .AddOption(new DialoguePassiveOption(id2, new string[] { "FIRST" }));
        output
            .AddEffect(new DialogueConditionalEffect().AddCondition("FIRST"))
            .AddHandler(new DialogueEffectFreeConditionHandler("FIRST"));
        return output;
    }

    private DialogueNode GenFight100(long id = 100, long victoryId = 3, long defeatId = 3) {
        DialogueCombatNode output = new DialogueCombatNode(id, combatUI);
        output
            .AddEffect()
            .AddHandler(new DialogueEffectPlayMusicHandler(1));
        output.victoryNodeId = victoryId;
        output.defeatNodeId = defeatId;

        output.startDialogue = new DialogueSpeech("An unpeted dogo appears!")
            .AddEffect()
            .AddHandler(new DialogueEffectSetSpriteSizeHandler(3, new Vector2(1, 1)))
            .AddHandler(new DialogueEffectSnapSpriteToPositionHandler(3, new Vector3(0, 0), SpriteSnapPosition.Bottom))
            .AddHandler(new DialogueEffectSetSpriteVisibleHandler(3, true));

        output.petDialogues = new DialogueNode[] { new DialogueSpeech("You heroically reach for the head, and pet!") };
        output.dogoPetDialogues = new DialogueNode[] {
            new DialogueSpeech("Dogo can't keep it together and gets loose.")
            .AddEffect()
            .AddHandler(new DialogueEffectMoveSpriteHandler(3, 0.1f, new Vector3(0.5f,0,0), SpriteSnapPosition.Bottom))
            .AddHandler(new DialogueEffectPlayAudioHandler(2))
            .AddHandler(new DialogueEffectWaitHandler(0.1f))
            .AddHandler(new DialogueEffectMoveSpriteHandler(3, 0.1f, new Vector3(-0.5f,0,0), SpriteSnapPosition.Bottom))
            .AddHandler(new DialogueEffectPlayAudioHandler(2))
            .AddHandler(new DialogueEffectWaitHandler(0.1f))
            .AddHandler(new DialogueEffectMoveSpriteHandler(3, 0.1f, new Vector3(0,0,0), SpriteSnapPosition.Bottom))
            .AddHandler(new DialogueEffectPlayAudioHandler(2))
            .AddHandler(new DialogueEffectWaitHandler(0.1f))
        };
        output.waitDialogues = new DialogueNode[] { new DialogueSpeech("You stand still, doing nothing.") };
        output.dogoWaitDialogues = new DialogueNode[] { new DialogueSpeech("Dogo is observing you, his tail moving in excited swipes.") };
        output.surrendDialogues = new DialogueNode[] { new DialogueSpeech("Hopeless, you sit on the floor.") };
        output.dogoSurrendDialogues = new DialogueNode[] { new DialogueSpeech("Dogo jump on top of you. You're his thing now!")
            .AddEffect()
            .AddHandler(new DialogueEffectScaleSpriteHandler(3, 0.3f, new Vector2(1.25f,1.25f)))
            .AddHandler(new DialogueEffectMoveSpriteHandler(3, 0.15f, new Vector3(0,0.2f,0), SpriteSnapPosition.Bottom))
            .AddHandler(new DialogueEffectPlayAudioHandler(2))
            .AddHandler(new DialogueEffectWaitHandler(0.15f))
            .AddHandler(new DialogueEffectMoveSpriteHandler(3, 0.15f, new Vector3(0,-0.1f,0), SpriteSnapPosition.Bottom))
            .AddHandler(new DialogueEffectPlayAudioHandler(2))
            .AddHandler(new DialogueEffectWaitHandler(0.15f))
        };
        output.victoryDialogue = new DialogueSpeech("You won! Dogo is going back home.")
            .AddEffect()
            .AddHandler(new DialogueEffectSetSpriteVisibleHandler(3, false));
        output.defeatDialogue = new DialogueSpeech("You were defeated!", "")
            .AddEffect()
            .AddHandler(new DialogueEffectSetSpriteVisibleHandler(3, false));
        dialogueContext.RegisterDialogueNode(id, output);
        return output;
    }

    private DialogueNode GenFight101(long id = 101) {
        DialogueVisualCombat output = new DialogueVisualCombat(id, visualCombatUI);
        output
            .AddEffect()
            .AddHandler(new DialogueEffectPlayMusicHandler(1));

        output.startDialogue = new DialogueSpeech("A clash of mighty warriors...");
            /*.AddEffect()
            .AddHandler(new DialogueEffectSetSpriteSizeHandler(3, new Vector2(1, 1)))
            .AddHandler(new DialogueEffectSnapSpriteToPositionHandler(3, new Vector3(0, 0), SpriteSnapPosition.Bottom))
            .AddHandler(new DialogueEffectSetSpriteVisibleHandler(3, true));*/

        output.victoryDialogue = new DialogueSpeech("You won! Dogo is going back home.")
            .AddEffect()
            .AddHandler(new DialogueEffectSetSpriteVisibleHandler(3, false));
        output.defeatDialogue = new DialogueSpeech("You were defeated!", "")
            .AddEffect()
            .AddHandler(new DialogueEffectSetSpriteVisibleHandler(3, false));
        dialogueContext.RegisterDialogueNode(id, output);
        return output;
    }

}