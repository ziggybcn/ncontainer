# NContainer #

#### Method Container.#ctor(NContainer.Container)

 Use this constructor to import data from other container upon container creation. 

|Name | Description |
|-----|------|
|containerToImport: ||


---
#### Method Container.Register``2

 Pairs an interface with a speciffic class. 

|Name | Description |
|-----|------|
|TP: |The interface|
|Name | Description |
|-----|------|
|TA: |The actual class that implements the interface|


---
#### Method Container.Register``1

 Paris a given class with each one of its interfaces 

|Name | Description |
|-----|------|
|TA: |The class to be registered|


---
#### Method Container.Register``1(``0)

 Pairs interface with a static instance of a class (useful for singleton-like needs) 

|Name | Description |
|-----|------|
|TP: |the interface|
|Name | Description |
|-----|------|
|instance: |the class intance implementing the interface|


---
#### Method Container.Register``1(System.Func{NContainer.Container,``0})

 Pairs an interface with a factory method 

|Name | Description |
|-----|------|
|TP: |The interface|
|Name | Description |
|-----|------|
|factory: |The factory method|


---
#### Method Container.IsRegistered``1

 Return True if the given interface has been registered into this container 

|Name | Description |
|-----|------|
|T: |The interface for ask for|
**Returns**: 



---
#### Method Container.GetAdapter(System.Type)

 Returns an instance of a registered class. Notice the generic version of this method is preferred always. 

|Name | Description |
|-----|------|
|contract: |The interface|


---
#### Method Container.GetAdapter``1

 Returns an instance of a registered class 

|Name | Description |
|-----|------|
|T: |The interface|


---


