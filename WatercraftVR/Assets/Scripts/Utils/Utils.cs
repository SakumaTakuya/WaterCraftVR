using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sakkun.Utility
{
	public static class RenderTextureUtil
	{
		public static Texture2D GetTexture(this RenderTexture renderTexture)
        {
            // アクティブなレンダーテクスチャをキャッシュしておく
            var currentRT = RenderTexture.active;

            // アクティブなレンダーテクスチャを一時的にTargetに変更する
            RenderTexture.active = renderTexture;

            // Texture2D.ReadPixels()によりアクティブなレンダーテクスチャのピクセル情報をテクスチャに格納する
            var texture = new Texture2D(renderTexture.width, renderTexture.height);
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();
                
            // アクティブなレンダーテクスチャを元に戻す
            RenderTexture.active = currentRT;

            return texture;
        }
	}

	public static class TransformUtil
	{
		public static Matrix4x4 GetTRS(this Transform transform)
		{
			return Matrix4x4.TRS
			(
				transform.position, 
				transform.rotation, 
				transform.lossyScale
			);
		}
	}

	public static class TupleEnumerable
	{

		public static IEnumerable<Tuple<T, int>> Indexed<T>(this IEnumerable<T> source)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			var i = 0;
			foreach(var item in source)
			{
				yield return new Tuple<T, int>(item, i);
				++i;
			}
		}

		public static T Item<T>(this Tuple<T, int> tuple)
		{
			return tuple.Item1;
		}

		public static int Index<T>(this Tuple<T, int> tuple)
		{
			return tuple.Item2;
		}
	}

	public static class IntUtils
	{
		public static string Ordinal(this int num, string format)
		{
			var numstr = num.ToString(format);
			return AddOrdinal(num , numstr);			
		}

		public static string Ordinal(this int num)
		{
			var numstr = num.ToString();
			return AddOrdinal(num , numstr);
		}

		private static string AddOrdinal(int num, string numstr)
		{
			if (num <= 0) return numstr;
            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return numstr + "th";
            }
            switch (num % 10)
            {
                case 1:
                    return numstr + "st";
                case 2:
                    return numstr + "nd";
                case 3:
                    return numstr + "rd";
                default:
                    return numstr + "th";
            }
		}
	}
}
