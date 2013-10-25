// test_exception.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#define BOOST_ENABLE_NON_INTRUSIVE_EXCEPTION_PTR

#include <boost/exception/detail/clone_current_exception.hpp>

#define BOOST_LIB_NAME boost_exception

#include <boost/config/auto_link.hpp>

#undef BOOST_LIB_NAME

int _tmain(int argc, _TCHAR* argv[])
{
    ::boost::exception_detail::clone_base const *p = nullptr;
    ::boost::exception_detail::clone_current_exception_non_intrusive(p);
	return 0;
}

