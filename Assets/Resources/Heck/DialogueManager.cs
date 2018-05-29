using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftDialogue;

public class DialogueManager : MonoBehaviour {

    public DialogueContext dialogueContext;

    void Start() {
        GenerateDialogues();
    }

    private void GenerateDialogues() {
        Debug.Log("GenerateDialogues");
        GenerateFugitive(10);
        GenerateStranger(20);
    }

    private DialogueNode GenerateFugitive(long id) {
        DialoguePassiveChoice root = dialogueContext.CreateDialoguePassiveChoice(id);
        root.AddOption(new DialoguePassiveOption(id + 1, new string[] { "!fugitive" }));
        root.AddOption(new DialoguePassiveOption(id + 4, new string[] { "fugitive" }));
        DialogueBranch firstEncounter = dialogueContext.CreateDialogueBranch(id + 1, id + 2);
        firstEncounter.AddEffect().AddHandler(new DialogueEffectSetConditionHandler("fugitive"));
        firstEncounter
            .AddNode(new DialogueSpeech("You... you..."))
            .AddNode(new DialogueSpeech("Oh, I'm so sorry..."))
            .AddNode(new DialogueSpeech("Such a waste..."))
            .AddNode(new DialogueSpeech("I'm not going to make it. Please, I beg you, hear me out."))
            .AddNode(new DialogueSpeech("I have a favour to ask of you."));

        DialogueActiveChoice firstEncounterChoice = dialogueContext.CreateDialogueActiveChoice(id + 2);
        firstEncounterChoice
            .AddOption(new DialogueActiveOption(id + 3, "Accept"))
            .AddOption(new DialogueActiveOption(id + 4, "Decline"));

        DialogueBranch firstEncounterAccept = dialogueContext.CreateDialogueBranch(id + 3, -1);
        firstEncounterAccept
            .AddEffect(new DialogueConditionalEffect()).AddCondition("!fugitiveAccept")
            .AddHandler(new DialogueEffectSetConditionHandler("fugitiveAccept"));
        firstEncounterAccept
            .AddNode(new DialogueSpeech("Oh..."))
            .AddNode(new DialogueSpeech("I thank you, from the bottom of my heart..."))
            .AddNode(new DialogueSpeech("Someone will come to claim me."))
            .AddNode(new DialogueSpeech("They will be here soon..."))
            .AddNode(new DialogueSpeech("Don't trust them."))
            .AddNode(new DialogueSpeech("Whatever they say to you, don't ever trust them."))
            .AddNode(new DialogueSpeech("Oppose them!"))
            .AddNode(new DialogueSpeech("They will trick you in doing... unspeakable things..."))
            .AddNode(new DialogueSpeech("Don't end..."))
            .AddNode(new DialogueSpeech("... like me..."));
        DialogueBranch firstEncounterDecline = dialogueContext.CreateDialogueBranch(id + 4, -1);
        firstEncounterDecline
            .AddEffect(new DialogueConditionalEffect()).AddCondition("!fugitiveDecline")
            .AddHandler(new DialogueEffectSetConditionHandler("fugitiveDecline"));
        firstEncounterDecline
            .AddNode(new DialogueSpeech("Oh, please..."))
            .AddNode(new DialogueSpeech("Don't be tricked..."))
            .AddNode(new DialogueSpeech("Save... yourself..."));
        return root;
    }

    private DialogueNode GenerateStranger(long id) {
        DialoguePassiveChoice root = dialogueContext.CreateDialoguePassiveChoice(id);
        root.AddOption(new DialoguePassiveOption(id + 1, new string[] { "!stranger" }));
        root.AddOption(new DialoguePassiveOption(id + 4, new string[] { "stranger" }));
        DialogueBranch firstEncounter = dialogueContext.CreateDialogueBranch(id + 1, id + 2);
        firstEncounter.AddEffect().AddHandler(new DialogueEffectSetConditionHandler("stranger"));
        firstEncounter
            .AddNode(new DialogueSpeech("Well, what do we have here?"))
            .AddNode(new DialogueSpeech("You've trapped yourself down here, have you not?"))
            .AddNode(new DialogueSpeech("A pity... You shouldn't go down a pit without thinking."));

        DialogueActiveChoice firstEncounterChoice = dialogueContext.CreateDialogueActiveChoice(id + 2);
        firstEncounterChoice
            .AddOption(new DialogueActiveOption(id + 3, "Accept"))
            .AddOption(new DialogueActiveOption(id + 4, "Decline"));

        DialogueBranch firstEncounterAccept = dialogueContext.CreateDialogueBranch(id + 3, -1);
        firstEncounterAccept
            .AddEffect(new DialogueConditionalEffect()).AddCondition("!fugitiveAccept")
            .AddHandler(new DialogueEffectSetConditionHandler("fugitiveAccept"));
        firstEncounterAccept
            .AddNode(new DialogueSpeech("Oh..."))
            .AddNode(new DialogueSpeech("I thank you, from the bottom of my heart..."))
            .AddNode(new DialogueSpeech("Someone will come to claim me."))
            .AddNode(new DialogueSpeech("They will be here soon..."))
            .AddNode(new DialogueSpeech("Don't trust them."))
            .AddNode(new DialogueSpeech("Whatever they say to you, don't ever trust them."))
            .AddNode(new DialogueSpeech("Oppose them!"))
            .AddNode(new DialogueSpeech("They will trick you in doing... unspeakable things..."))
            .AddNode(new DialogueSpeech("Don't end..."))
            .AddNode(new DialogueSpeech("... like me..."));
        DialogueBranch firstEncounterDecline = dialogueContext.CreateDialogueBranch(id + 4, -1);
        firstEncounterDecline
            .AddEffect(new DialogueConditionalEffect()).AddCondition("!fugitiveDecline")
            .AddHandler(new DialogueEffectSetConditionHandler("fugitiveDecline"));
        firstEncounterDecline
            .AddNode(new DialogueSpeech("Oh, please..."))
            .AddNode(new DialogueSpeech("Don't be tricked..."))
            .AddNode(new DialogueSpeech("Save... yourself..."));
        return root;
    }
}