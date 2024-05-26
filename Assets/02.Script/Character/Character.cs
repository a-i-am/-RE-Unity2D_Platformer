using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] // ������ ����ȭ(����Ʈ ������� Inspector�� ǥ������)
public class Character
{
    [System.Serializable] // ������ ����ȭ(����Ʈ ������� Inspector�� ǥ������)
    public class CharacterData
    {
        public CharacterData(string _type, string _name, string _explain, string _number, bool _isUsing, string _tabName = "Character")
        { tabName = _tabName; type = _type; name = _name; explain = _explain; number = _number; isUsing = _isUsing; }

        public string tabName, type, name, explain, number; // string�̾�� JSON �Ľ� �� �� �ȴٰ� ��
        public bool isUsing;
        public Sprite characterImage;
        public List<CharacterAbility> abilities;

        //public bool UseCharacter()
        //{
        //    isUsing = false;
        //    foreach (CharacterAbility ability in abilities)
        //    {
        //        isUsing = ability.ExecuteRole();
        //    }
        //    return isUsing;
        //}
    }

}