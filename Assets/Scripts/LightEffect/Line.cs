using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.LightEffect
{
    public class Line : MonoBehaviour
    {
        public Vector2 Start;
        
        public Vector2 End;

        public float Thickness;

        public GameObject StartCapChild;
        public GameObject LineChild;
        public GameObject EndCapChild;

        public Line(Vector2 start, Vector2 end, float thickness)
        {
            Start = start;
            End = end;
            Thickness = thickness;
        }

        public void SetColor(Color color)
        {
            StartCapChild.GetComponent<SpriteRenderer>().color = color;
            LineChild.GetComponent<SpriteRenderer>().color = color;
            EndCapChild.GetComponent<SpriteRenderer>().color = color;
        }

        public void Draw()
        {
            Vector2 difference = End - Start;
            float rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            //Set the scale of the line to reflect length and thickness
            LineChild.transform.localScale = new Vector3(100 * (difference.magnitude / LineChild.GetComponent<SpriteRenderer>().sprite.rect.width),
                                                         Thickness,
                                                         LineChild.transform.localScale.z);

            StartCapChild.transform.localScale = new Vector3(StartCapChild.transform.localScale.x,
                                                             Thickness,
                                                             StartCapChild.transform.localScale.z);

            EndCapChild.transform.localScale = new Vector3(EndCapChild.transform.localScale.x,
                                                           Thickness,
                                                           EndCapChild.transform.localScale.z);

            //Rotate the line so that it is facing the right direction
            LineChild.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
            StartCapChild.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
            EndCapChild.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation + 180));

            //Move the line to be centered on the starting point
            LineChild.transform.position = new Vector3(Start.x, Start.y, LineChild.transform.position.z);
            StartCapChild.transform.position = new Vector3(Start.x, Start.y, StartCapChild.transform.position.z);
            EndCapChild.transform.position = new Vector3(Start.x, Start.y, EndCapChild.transform.position.z);

            //Need to convert rotation to radians at this point for Cos/Sin
            rotation *= Mathf.Deg2Rad;

            //Store these so we only have to access once
            float lineChildWorldStartdjust = LineChild.transform.localScale.x * LineChild.GetComponent<SpriteRenderer>().sprite.rect.width / 2f;
            float startCapChildWorldStartdjust = StartCapChild.transform.localScale.x * StartCapChild.GetComponent<SpriteRenderer>().sprite.rect.width / 2f;
            float endCapChildWorldStartdjust = EndCapChild.transform.localScale.x * EndCapChild.GetComponent<SpriteRenderer>().sprite.rect.width / 2f;

            //Startdjust the middle segment to the appropriate position
            LineChild.transform.position += new Vector3(.01f * Mathf.Cos(rotation) * lineChildWorldStartdjust,
                                                         .01f * Mathf.Sin(rotation) * lineChildWorldStartdjust,
                                                         0);

            //Startdjust the start cap to the appropriate position
            StartCapChild.transform.position -= new Vector3(.01f * Mathf.Cos(rotation) * startCapChildWorldStartdjust,
                                                             .01f * Mathf.Sin(rotation) * startCapChildWorldStartdjust,
                                                             0);

            //Startdjust the end cap to the appropriate position
            EndCapChild.transform.position += new Vector3(.01f * Mathf.Cos(rotation) * lineChildWorldStartdjust * 2,
                                                           .01f * Mathf.Sin(rotation) * lineChildWorldStartdjust * 2,
                                                           0);
            EndCapChild.transform.position += new Vector3(.01f * Mathf.Cos(rotation) * endCapChildWorldStartdjust,
                                                           .01f * Mathf.Sin(rotation) * endCapChildWorldStartdjust,
                                                           0);
        }
    }
}
