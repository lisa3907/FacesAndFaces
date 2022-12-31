cd ./src/Web/WebMvc
docker build --tag tkousek/mvc .
docker push tkousek/mvc
kubectl delete deployment webmvc
kubectl delete service webmvc
cd /Users/tkousek/AspNetCoreMicro/ShoesOnContainers/kompose
kubectl apply -f webmvc-deployment.yaml
kubectl expose deployment webmvc --type=LoadBalancer --name=webmvc
