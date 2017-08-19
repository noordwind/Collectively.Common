#!/bin/bash
echo Executing after success scripts on branch $TRAVIS_BRANCH
echo Triggering MyGet package build

case "$TRAVIS_BRANCH" in
  "master")
    echo Triggering MyGet package build using branch $TRAVIS_BRANCH
    curl -X POST -d '{}' "$MYGET_TRIGGER_BUILD_PACKAGE_URL"
    ;;
  "develop")
    echo Triggering MyGet package build using branch $TRAVIS_BRANCH
    curl -X POST -d '{}' "$MYGET_TRIGGER_BUILD_PACKAGE_DEV_URL"
    ;;    
esac