#!/bin/bash

while getopts p:n:f: flag
do
    case "${flag}" in
        p) path=${OPTARG};;
        n) name=${OPTARG};;
	    f) prefix=${OPTARG};;
    esac
done

cd $path;
rm -rf $name
mkdir $name;
cd ./$name

touch "${name}Dto.cs";
touch "${name}Request.cs";
touch "${name}Response.cs";
touch "I${name}Service.cs";


for file in *
do
	if [ ! -s "$file" ]
	then
        echo "Prossing file: $file"
        class_name="${file%%.*}"
        echo "Class name: $class_name"
        if [[ "$file" == *"Service"* ]]
        then
            echo "namespace ${prefix}.${name};" >> $file
            echo "" >> $file
            echo "public interface ${class_name}" >> $file
            echo "{" >> $file
            echo "" >> $file
            echo "}" >> $file
        else
            echo "namespace ${prefix}.${name};" >> $file
            echo "" >> $file
            echo "public static class ${class_name}" >> $file
            echo "{" >> $file
            echo "" >> $file
            echo "}" >> $file
        fi
    fi
done