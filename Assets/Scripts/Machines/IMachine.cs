using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IMachine
{
    void clearContents();
	void onTick();
	bool addInput(Item input, int count);
	(Item item, int count) getOutput();
}