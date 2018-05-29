using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heck {

    public class IllusoryWall : AttackableObject {

        public Material[] fadingMaterials;

        public override void Die() {
            GetComponent<MeshRenderer>().materials = fadingMaterials;
            FadeObject();
        }

    }

}
