using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace MonsterAdventure
{
    /*
     * \brief   It is a container to store an object
     *          and it's collision bounds (a rect).
     */
    public class QTObject<T>
    {
        public T obj;
        public Rect rect;

        public QTObject(T obj, Rect rect)
        {
            this.obj = obj;
            this.rect = rect;
        }

        public override string ToString()
        {
            return "QTObject : " + obj.ToString();
        }
    }

    /*
     * \brief   A class which provide a first traitment
     *          to accelerate collision computing in 2D.
     */
    public class QuadTree<T>
    {
        public static int MAX_OBJECTS = 30;
        public static int MAX_LEVELS = 5;

        private int _level;
        private List<QTObject<T>> _objects;
        private Rect _bounds;
        private QuadTree<T>[] _nodes;

        public QuadTree(int level, Rect _bounds)
        {
            _level = level;
            _objects = new List<QTObject<T>>();
            this._bounds = _bounds;
            _nodes = new QuadTree<T>[4];
        }

        /*!
         * \brief   clears the quadtree by recursively 
         *          clearing all objects from all nodes.
        */
        public void Clear()
        {
            _objects.Clear();

            for (int i = 0; i < _nodes.Length; i++)
            {
                if (_nodes[i] != null)
                {
                    _nodes[i].Clear();
                    _nodes[i] = null;
                }
            }
        }

        /*!
         * \brief   Remove from the QuadTree an Object
         * \param   The object to remove
         * \return  true if the object was removed (and found !),
         *          false otherwise
         */
        public bool Remove(T obj)
        {
            QTObject<T> _object = _objects.Find(x => x.obj.Equals(obj));
            if (_object != null)
            {
                if (_objects.Remove(_object))
                {
                    return true;
                }
            }
            else if (_nodes[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (_nodes[i].Remove(obj))
                        return true;
                }
            }

            return false;
        }

        /*!
         * \brief   splits the node into four subnodes by dividing 
         *          the node into four equal parts and initializing 
         *          the four subnodes with the new bounds.
         */
        private void Split()
        {
            int subWidth = (int)(_bounds.width / 2);
            int subHeight = (int)(_bounds.height / 2);
            int x = (int)_bounds.x;
            int y = (int)_bounds.y;

            _nodes[0] = new QuadTree<T>(_level + 1,
                new Rect(x + subWidth, y, subWidth, subHeight));
            _nodes[1] = new QuadTree<T>(_level + 1,
                new Rect(x, y, subWidth, subHeight));
            _nodes[2] = new QuadTree<T>(_level + 1,
                new Rect(x, y + subHeight, subWidth, subHeight));
            _nodes[3] = new QuadTree<T>(_level + 1,
                new Rect(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        /*
         * \brief   Determine which node the object belongs to.
         * \param   a_Rect The hitbox of the object.
         * \return  The index of the QuadTree which the node belong to, or -1
         *          if there is no completely matching
         */
        private int GetIndex(Rect rect)
        {
            int index = -1;
            double verticalMidPoint = _bounds.x + (_bounds.width / 2);
            double horizontalMidPoint = _bounds.y + (_bounds.height / 2);

            // Object can completely fit within the top quadrants
            bool topQuadrant = (rect.y < horizontalMidPoint
                && rect.y + rect.height < horizontalMidPoint);

            // Object can completely fit within the bottom quadrants
            bool botQuadrant = (rect.y > horizontalMidPoint);

            if (rect.x < verticalMidPoint && rect.x + rect.width < verticalMidPoint)
            {
                if (topQuadrant)
                    index = 1;

                else if (botQuadrant)
                    index = 2;
            }

            // Object can completely fit within the right quadrants
            else if (rect.x > verticalMidPoint)
            {
                if (topQuadrant)
                    index = 0;

                else if (botQuadrant)
                    index = 3;
            }

            return index;
        }

        /*
         * \brief   Insert the object into the quadtree. If the node
         *          exceeds the capacity, it will split and add all
         *          objects to their corresponding nodes.
         * \param   a_Object The object to insert
         */
        public void Insert(QTObject<T> obj)
        {
            if (_nodes[0] != null)
            {
                int index = GetIndex(obj.rect);

                if (index != -1)
                {
                    _nodes[index].Insert(obj);
                    return;
                }
            }

            _objects.Add(obj);

            if (_objects.Count > MAX_OBJECTS && _level < MAX_LEVELS)
            {
                if (_nodes[0] == null)
                    Split();

                int i = 0;

                while (i < _objects.Count)
                {
                    int index = GetIndex(_objects[i].rect);
                    if (index != -1)
                    {
                        QTObject<T> qtObject = _objects[i];
                        _objects.RemoveAt(i);
                        _nodes[index].Insert(qtObject);
                    }
                    else
                    {
                        i++;
                    }

                }
            }
        }

        /*
         * \brief   Find all potential collisions for specific bounds
         * \param   a_ReturnObject the list of QTObject in collision
         *          with the specific bounds.
         * \param   a_Rect the specific bounds (i.e. the bounds of the
         *          object we want to test)
         * \return  the list of collided objects (use for recursive operation),
         *          which be the same as a_ReturnObjects at the end.
         */
        public List<QTObject<T>> Retrieve(List<QTObject<T>> returnObjects, Rect rect)
        {
            if (_nodes[0] != null)
            {
                int index = GetIndex(rect);

                if (index != -1)
                {
                    _nodes[index].Retrieve(returnObjects, rect);
                }
                else
                {
                    for (int i = 0; i < _nodes.Length; i++)
                    {
                        _nodes[i].Retrieve(returnObjects, rect);
                    }
                }
            }

            returnObjects.AddRange(_objects);

            return returnObjects;
        }
    }
}