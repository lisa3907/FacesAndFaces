# run these 
kubectl apply -f mssqlserver-deployment.yaml
kubectl apply -f mssqlserver-service.yaml
kubectl apply -f rabbitmq-claim0-persistentvolumeclaim.yaml
kubectl apply -f rabbitmq-deployment.yaml
kubectl apply -f rabbitmq-service.yaml
kubectl apply -f basketdata-deployment.yaml
kubectl apply -f basketdata-service.yaml


# Stop here and wait a while before you run the tokenserver

kubectl apply -f tokenserver-deployment.yaml
kubectl apply -f tokenserver-service.yaml

kubectl apply -f cart-deployment.yaml
kubectl apply -f cart-service.yaml

# Stop here and wait a while before you run the next

kubectl apply -f order-deployment.yaml
kubectl apply -f order-service.yaml

kubectl apply -f catalog-deployment.yaml
kubectl apply -f catalog-service.yaml

kubectl apply -f webmvc-deployment.yaml
#kubectl apply -f webmvc-service.yaml

kubectl expose deployment webmvc --type=LoadBalancer --name=webmvc

