using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    [CreateAssetMenu(fileName = "New MoveSet", menuName = "Heck/MoveSet")]
    public class MoveSet : ScriptableObject {

        public string setName;

        public CharacterMove[] attackMoves;
        public CharacterMove[] bashMoves;
    }
}
