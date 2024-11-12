using System.Collections;

public class MyVector
{
	private readonly ArrayList a;

	public MyVector()
	{
		a = new ArrayList();
	}

	public MyVector(ArrayList a)
	{
		this.a = a;
	}

	public void AddElement(object o)
	{
		a.Add(o);
	}

	public bool Contains(object o)
	{
		if (a.Contains(o))
		{
			return true;
		}
		return false;
	}

	public int Size()
	{
		if (a == null)
		{
			return 0;
		}
		return a.Count;
	}

	public object ElementAt(int index)
	{
		if (index > -1 && index < a.Count)
		{
			return a[index];
		}
		return null;
	}

	public void Set(int index, object obj)
	{
		if (index > -1 && index < a.Count)
		{
			a[index] = obj;
		}
	}

	public void SetElementAt(object obj, int index)
	{
		if (index > -1 && index < a.Count)
		{
			a[index] = obj;
		}
	}

	public int IndexOf(object o)
	{
		return a.IndexOf(o);
	}

	public void RemoveElementAt(int index)
	{
		if (index > -1 && index < a.Count)
		{
			a.RemoveAt(index);
		}
	}

	public void RemoveElement(object o)
	{
		a.Remove(o);
	}

	public void RemoveAllElements()
	{
		a.Clear();
	}

	public void InsertElementAt(object o, int i)
	{
		a.Insert(i, o);
	}

	public object FirstElement()
	{
		return a[0];
	}

	public object LastElement()
	{
		return a[a.Count - 1];
	}
}
