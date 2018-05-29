using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Heck {

    [CreateAssetMenu(fileName = "New CharacterMove", menuName = "Heck/CharacterMove")]
    public class CharacterMove : ScriptableObject {

        public string animationName = "animation";
        public bool isCancelable = false;
        public bool allowsRotation = false;
        public float animationDuration;
        public int energyCost = 0;

        public FightMoveChaining[] chainMoves;

        public string[] interactionStrings;

        private MoveInteraction[] interactions;
        public MoveInteraction[] GetInteractions() {
            if (interactions == null) {
                interactions = new MoveInteraction[interactionStrings.Length];
                for (int i = 0; i < interactionStrings.Length; ++i) {
                    interactions[i] = MoveInteraction.StringToInteraction(interactionStrings[i].Replace(" ", ""));
                }
            }
            return interactions;
        }

    }

    public abstract class MoveInteraction {
        // common
        public Vector2 timeStep;
        // interface
        public abstract bool CanCancel();
        public abstract IEnumerator DoInteraction(Character character, CharacterMove move);

        private static Regex classRegex = new Regex("([^\\(\\)].*)\\((.*)\\)");
        private static Regex argsRegex = new Regex("([^,]+)");

        public static MoveInteraction StringToInteraction(string str) {
            Match matchClass = classRegex.Match(str);
            if (matchClass.Success) {
                string className = matchClass.Groups[1].Value;
                string args = matchClass.Groups[2].Value;
                Match matchArgs = argsRegex.Match(args);
                float x = float.Parse(matchArgs.Value); matchArgs = matchArgs.NextMatch();
                float y = float.Parse(matchArgs.Value); matchArgs = matchArgs.NextMatch();
                MoveInteraction output;
                switch (className) {
                    case "WeaponDamage":
                        MoveWeaponDamageInteraction mwdi = new MoveWeaponDamageInteraction();
                        mwdi.damageRatio = float.Parse(matchArgs.Value); matchArgs = matchArgs.NextMatch();
                        output = mwdi;
                        break;
                    case "Evasion":
                        MoveEvasionInteraction mei = new MoveEvasionInteraction();
                        output = mei;
                        break;
                    case "Vulnerability":
                        MoveVulnerabilityInteraction mvi = new MoveVulnerabilityInteraction();
                        output = mvi;
                        break;
                    case "Displacement":
                        MoveDisplacementInteraction mdi = new MoveDisplacementInteraction();
                        mdi.distance.x = float.Parse(matchArgs.Value); matchArgs = matchArgs.NextMatch();
                        mdi.distance.y = float.Parse(matchArgs.Value); matchArgs = matchArgs.NextMatch();
                        mdi.distance.z = float.Parse(matchArgs.Value); matchArgs = matchArgs.NextMatch();
                        output = mdi;
                        break;
                    case "AudioClip":
                        MoveAudioClipInteraction maci = new MoveAudioClipInteraction();
                        maci.clip = Resources.Load<AudioClip>(matchArgs.Value); matchArgs = matchArgs.NextMatch();
                        if (matchArgs.Success) { maci.canCancel = bool.Parse(matchArgs.Value); matchArgs = matchArgs.NextMatch(); }
                        output = maci;
                        break;
                    case "Pattern":
                        MovePatternInteraction mpi = new MovePatternInteraction();
                        mpi.pattern = (Pattern)Resources.Load(matchArgs.Value); matchArgs = matchArgs.NextMatch();
                        output = mpi;
                        break;
                    default:
                        throw new System.Exception();
                }
                output.timeStep.x = x;
                output.timeStep.y = y;
                return output;
            }
            throw new System.Exception();
        }

    }

    [System.Serializable]
    public class MoveWeaponDamageInteraction : MoveInteraction {
        // unique
        public float damageRatio;
        // interface
        public override bool CanCancel() {
            return true;
        }
        public override IEnumerator DoInteraction(Character character, CharacterMove move) {
            yield return new WaitForSeconds(timeStep.x);
            character.SetDamageFrame(true, damageRatio);
            yield return new WaitForSeconds(timeStep.y);
            character.SetDamageFrame(false);
        }
    }

    [System.Serializable]
    public class MoveEvasionInteraction : MoveInteraction {
        // interface
        public override bool CanCancel() {
            return true;
        }
        public override IEnumerator DoInteraction(Character character, CharacterMove move) {
            yield return new WaitForSeconds(timeStep.x);
            character.SetEvasionFrame(true);
            yield return new WaitForSeconds(timeStep.y);
            character.SetEvasionFrame(false);
        }
    }

    [System.Serializable]
    public class MoveVulnerabilityInteraction : MoveInteraction {
        // interface
        public override bool CanCancel() {
            return true;
        }
        public override IEnumerator DoInteraction(Character character, CharacterMove move) {
            yield return new WaitForSeconds(timeStep.x);
            character.SetVulnerabilityFrame(true);
            yield return new WaitForSeconds(timeStep.y);
            character.SetVulnerabilityFrame(false);
        }
    }

    [System.Serializable]
    public class MoveDisplacementInteraction : MoveInteraction {
        // unique
        public Vector3 distance;
        // interface
        public override bool CanCancel() {
            return true;
        }
        public override IEnumerator DoInteraction(Character character, CharacterMove move) {
            float elapsedTime = 0;
            float timeOverflow;
            while (elapsedTime < timeStep.x) { // while not in displacement
                yield return null;
                elapsedTime += Time.deltaTime;
            }
            timeOverflow = elapsedTime - timeStep.x;
            if (timeOverflow > timeStep.y) { // just apply movement, and skip to next displacement
                character.ApplyDisplacement(distance);
            } else {
                character.ApplyDisplacement(distance * timeOverflow / timeStep.y);
                float endTime = timeStep.x + timeStep.y;
                while (elapsedTime < endTime) {
                    yield return null;
                    elapsedTime += Time.deltaTime;
                    if (elapsedTime < endTime) {
                        character.ApplyDisplacement(distance * Time.deltaTime / timeStep.y);
                    } else {
                        timeOverflow = (timeStep.x + timeStep.y) - (elapsedTime - Time.deltaTime);
                        character.ApplyDisplacement(distance * timeOverflow / timeStep.y);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class MoveAudioClipInteraction : MoveInteraction {
        // unique
        public AudioClip clip;
        public bool canCancel = false;
        // interface
        public override bool CanCancel() {
            return canCancel;
        }
        public override IEnumerator DoInteraction(Character character, CharacterMove move) {
            yield return new WaitForSeconds(timeStep.x);
            character.PlayAudioClip(clip);
        }
    }

    [System.Serializable]
    public class MovePatternInteraction : MoveInteraction {
        public Pattern pattern;
        // interface
        public override bool CanCancel() {
            return false;
        }
        public override IEnumerator DoInteraction(Character character, CharacterMove move) {
            return pattern.DoPattern(character, character.transform, this);
        }
    }

    [System.Serializable]
    public class FightMoveChaining {
        public InputCommand command;
        public CharacterMove move;
    }

}

