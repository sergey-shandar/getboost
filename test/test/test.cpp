// test.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <algorithm>
#include <boost/archive/iterators/transform_width.hpp>

int _tmain(int argc, _TCHAR* argv[])
{
	boost::archive::iterators::transform_width<int*, 0, 8> *x = nullptr;
	x->operator*();
	return 0;
}

