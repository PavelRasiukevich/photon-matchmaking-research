using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class RoomInfoList<T> : List<T>
    {
        private List<T> _list;

        public List<T> List
        {
            get => _list;
            set => _list = value;
        }

        public RoomInfoList()
        {
            _list = new List<T>();
        }

        public RoomInfoList<T> Open()
        {
            return this;
        }
    }
}