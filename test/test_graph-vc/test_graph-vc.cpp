// test_graph-vc.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <boost/graph/graphml.hpp>

#define BOOST_LIB_NAME boost_graph

#include <boost/config/auto_link.hpp>

#undef BOOST_LIB_NAME

int _tmain(int argc, _TCHAR* argv[])
{
	::boost::mutate_graph *g = nullptr;
	::boost::read_graphml(std::cin, *g, 0);
	return 0;
}

